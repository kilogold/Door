using Nethereum.Web3;

namespace dotnet
{
    public partial class Runtime
    {
        public class Blackboard
        {
            private Blackboard(){}
            public static Blackboard Instance { get; } = new();

            public Web3 web3 = null;
            public string posContractAddress = null;

        }
    }
}