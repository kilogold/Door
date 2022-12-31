using System.Numerics;
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
    }
}