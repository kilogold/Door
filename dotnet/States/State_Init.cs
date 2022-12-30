namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_Init : BaseState
        {
           public static State_Init Instance { get; } = new();
           private State_Init(){}
           public override void Init()
           {
               DisplayMessage = "Initializing Web3...";
               Options = OptionTemplate_Continue(State_ContractDeploy.Instance);
           }

            public override Task Enter()
            {
                Blackboard.Instance.Init();
                return Task.CompletedTask;
            }
        }
    }
}