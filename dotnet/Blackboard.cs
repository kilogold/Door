using System.Diagnostics;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

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
            
            public Door door = new();
            public Web3 web3 { get; private set; }
            public string posContractAddress = null;
            
            public void Init()
            {     
                Debug.Assert(web3 == null);
                const string KEY_ENV_VAR = "POS_DOOR_PRIV_KEY";
                
                var privateKey = Environment.GetEnvironmentVariable(KEY_ENV_VAR);
                if (string.IsNullOrEmpty(privateKey))
                    throw new Exception($"Environment variable {KEY_ENV_VAR} is not defined.");
                
                var account = new Account(privateKey);
                web3 = new Web3(account)
                {
                    TransactionManager =
                    {
                        UseLegacyAsDefault = true
                    }
                };
            }

        }
    }
}