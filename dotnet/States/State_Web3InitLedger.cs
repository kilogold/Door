namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_Web3InitLedger : BaseState
        {
            public static State_Web3InitLedger Instance { get; } = new();
            private State_Web3InitLedger() { }

            public override void Init()
            {
                DisplayMessage = "Initializing Web3...";
                Options = string.IsNullOrEmpty(ProgramConfig.predefinedContractAddress)
                    ? OptionTemplate_Continue(State_ContractDeploy.Instance)
                    : OptionTemplate_Continue(State_ContractLoad.Instance);
            }

            public override async Task Processing()
            {
                var fundingSource = Utils.ProduceWeb3FromEnv("POS_USER_PRIV_KEY");
                
                Blackboard.Instance.web3 = await Utils.ProduceWeb3FromLedgerDevice(
                    ProgramConfig.rpcClientUri, 
                    ProgramConfig.chainId, 
                    fundingSource);
            }
        }
    }
}