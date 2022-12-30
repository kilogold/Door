namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_DoorInteraction : BaseState
        {
            private State_DoorInteraction()
            {
                DisplayMessage = "The door stands before you. What will you do?";
                Options = new[]
                {
                    new IState.Option("Turn the handle.", State_DoorCheck.Instance)
                };
            }

            public static State_DoorInteraction Instance { get; } = new();
        }
    }
}