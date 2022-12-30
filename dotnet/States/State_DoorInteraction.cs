namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_DoorInteraction : BaseState
        {
            public static State_DoorInteraction Instance { get; } = new();
            private State_DoorInteraction(){}            
            public override void Init()
            {
                DisplayMessage = "The door stands before you. What will you do?";
                Options = new[]
                {
                    new IState.Option("Turn the handle.", State_DoorCheck.Instance)
                };            
            }

        }
    }
}