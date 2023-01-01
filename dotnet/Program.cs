using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace dotnet
{
    public static class ProgramConfig
    {
        public static string predefinedContractAddress = null;
        public static readonly BigInteger CONFIG_RATE = Web3.Convert.ToWei(100);

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

        static async Task<int> LedgerDevice()
        {
            Web3 extWeb3 = null;
            Web3 embeddedWeb3 = Utils.ProduceWeb3FromEnv("POS_USER_PRIV_KEY");

            Console.WriteLine("Please confirm the account address you will use on your Ledger device.");
            try
            {
                extWeb3 = await Utils.ProduceWeb3FromLedgerDevice("http://localhost:8545", 1337, embeddedWeb3);
            }
            catch (Exception e)
            {
                if (e.Message.Equals("Shrugging in your general direction"))
                {
                    Console.WriteLine($"Unknown error. Is your device connected, unlocked, and running the Ethereum app?");
                }
                else
                {
                    Console.WriteLine("Abort. {0}", e.Message);
                }

                return -1;
            }

            Console.WriteLine("Confirmed. The address used for transaction signing is: " + extWeb3.TransactionManager.Account.Address);
            var loadedBalance = await extWeb3.Eth.GetBalance.SendRequestAsync(extWeb3.TransactionManager.Account.Address);
            Console.WriteLine("Address is funded with {0} ETH from {1}",
                Web3.Convert.FromWei(loadedBalance),
                embeddedWeb3.TransactionManager.Account.Address);

            // Give half back.
            {
                var txnInput = new TransactionInput();
                txnInput.From = extWeb3.TransactionManager.Account.Address;
                txnInput.To = embeddedWeb3.TransactionManager.Account.Address;
                txnInput.Gas = new HexBigInteger(50000);
                txnInput.GasPrice = new HexBigInteger(50000000000);
                txnInput.Value = new HexBigInteger(Web3.Convert.ToWei(10));

                var receipt = await extWeb3.TransactionManager.SendTransactionAndWaitForReceiptAsync(txnInput);

                Console.WriteLine("Transaction successful:\n" + receipt.TransactionHash);
            }
            return 0;
        }
    }
}