using System.Diagnostics;
using Nethereum.Web3;

namespace dotnet
{
    public partial class Runtime
    {
        private class Door
        {
            public enum State {Undefined, Locked, Unlocked}

            public State CurrentState { get; set; } = State.Undefined;
        }
        
        private class Blackboard
        {
            private Blackboard(){}
            public static Blackboard Instance { get; } = new();
            
            public readonly Door door = new();
            public Web3 web3 { get; set; }
            public string posContractAddress = null;
            
        }
    }
}