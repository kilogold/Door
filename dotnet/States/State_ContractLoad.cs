namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_ContractLoad : BaseState
        {
            public static State_ContractLoad Instance { get; } = new();
            private State_ContractLoad(){ }

            public override void Init()
            {
                DisplayMessage = "Loading contract from config...\n" + ProgramConfig.predefinedContractAddress;
                Options = OptionTemplate_Continue(State_DoorInteraction.Instance);
            }

            public override Task Processing()
            {
                Blackboard.Instance.posContractAddress = ProgramConfig.predefinedContractAddress;
                return Task.CompletedTask;
            }
        }
    }
}