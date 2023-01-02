using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace dotnet
{
    public static class ProgramConfig
    {
        public static bool fundLedgerFromEnv = false;
        public static string ledgerFundingEnvPrivateKey = "POS_LEDGER_FUNDING_PRIV_KEY";
        public const string rpcClientUri = "https://sepolia.infura.io/v3/[YOUR PROJECT ID HERE]";
        public const int chainId = 11155111;
        public static string predefinedContractAddress = "0x7bc3579c9d0ed872deb7f9515dbf1c7235ee8bdc";
        public static readonly BigInteger CONFIG_RATE = Web3.Convert.ToWei(0.0001);

        private static bool IsValidEVMAddress(string input)
        {
            return true;
        }
        
        public static void OverrideFromArgs(string[] args)
        {
            if(IsValidEVMAddress(args[0]))
            {
                predefinedContractAddress = args[0];
            }
        }
    }
    
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            if (args.Length > 0)
            {
                ProgramConfig.OverrideFromArgs(args);
            }
            
            var runtime = new Runtime();
            while (runtime.IsRunning)
            {
                runtime.PrintStateMessage();
                await runtime.Processing();
                runtime.PrintStateOptions();
                await runtime.Input();
            }
            return 0;
        }
    }
}