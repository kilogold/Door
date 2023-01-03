using Nethereum.Web3;

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
                Options = string.IsNullOrEmpty(ProgramConfig.chainSettings.predefinedContractAddress)
                    ? OptionTemplate_Continue(State_ContractDeploy.Instance)
                    : OptionTemplate_Continue(State_ContractLoad.Instance);
            }

            public override async Task Processing()
            {
                Blackboard.Instance.web3 = await Utils.ProduceWeb3FromLedgerDevice();
                
                if (ProgramConfig.fundLedgerFromEnv)
                {
                    var fundingSource = Utils.ProduceWeb3FromEnv(ProgramConfig.ledgerFundingEnvPrivateKey);
                    
                    Console.WriteLine($"Financing Ledger device account ({Blackboard.Instance.web3.TransactionManager.Account.Address})\n" +
                    $"with funding account ({fundingSource.TransactionManager.Account.Address})");

                    await Utils.TransferFullBalance(fundingSource, Blackboard.Instance.web3);
                }
            }
        }
    }
}