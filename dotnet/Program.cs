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
        public static readonly BigInteger CONFIG_RATE = Web3.Convert.ToWei(0.001);
        public static readonly BlockchainConnectionSettings chainSettings = new DevChainSettings();

        private static bool IsValidEVMAddress(string input)
        {
            return true;
        }
        
        public static void OverrideFromArgs(string[] args)
        {
            if(IsValidEVMAddress(args[0]))
            {
                chainSettings.predefinedContractAddress = args[0];
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