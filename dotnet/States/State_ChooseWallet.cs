namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_ChooseWallet : BaseState
        {
            public static State_ChooseWallet Instance { get; } = new();
            private State_ChooseWallet() { }

            public override void Init()
            {
                DisplayMessage = "Choose your wallet type.";
                Options = new[]
                {
                    new IState.Option("External wallet (Ledger)", State_Web3InitLedger.Instance),
                    new IState.Option("Embedded wallet (Private key)", State_Web3Init.Instance),
                };
            }
        }
    }
}