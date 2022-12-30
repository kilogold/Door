namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_Init : BaseState
        {
            private State_Init()
            {
                DisplayMessage = "Initializing Web3...";
                Options = OptionTemplate_Continue(State_ContractDeploy.Instance);
            }

            public static State_Init Instance { get; } = new();

            public override Task Enter()
            {
                Blackboard.Instance.Init();
                return Task.CompletedTask;
            }
        }
    }
}