using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace dotnet
{
    public partial class Runtime
    {
        private class State_Init : BaseState
        {
            public State_Init()
            {
                DisplayMessage = "Initializing Web3 accounts...";
                Options = OptionTemplate_Continue(new State_ContractDeploy(10));
            }
            
            public override Task Processing()
            {
                const string KEY_ENV_VAR = "POS_DOOR_PRIV_KEY";
                
                var privateKey = Environment.GetEnvironmentVariable(KEY_ENV_VAR);
                if (string.IsNullOrEmpty(privateKey))
                    throw new Exception($"Environment variable {KEY_ENV_VAR} is not defined.");
                
                var account = new Account(privateKey);
                var web3 = new Web3(account)
                {
                    TransactionManager =
                    {
                        UseLegacyAsDefault = true
                    }
                };

                Blackboard.Instance.web3 = web3;
                return Task.CompletedTask;
            }
        }
    }
}