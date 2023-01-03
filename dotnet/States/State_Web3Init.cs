namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_Web3Init : BaseState
        {
           public static State_Web3Init Instance { get; } = new();
           private State_Web3Init(){}
           public override void Init()
           {
               DisplayMessage = "Initializing Web3...";
               
               Options = string.IsNullOrEmpty(ProgramConfig.chainSettings.predefinedContractAddress)
                   ? OptionTemplate_Continue(State_ContractDeploy.Instance)
                   : OptionTemplate_Continue(State_ContractLoad.Instance);
           }

            public override Task Processing()
            {
                Blackboard.Instance.web3 = Utils.ProduceWeb3FromEnv("POS_USER_PRIV_KEY");
                return Task.CompletedTask;
            }
        }
    }
}