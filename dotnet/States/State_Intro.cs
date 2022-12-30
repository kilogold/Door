namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_Intro : BaseState
        {
            private State_Intro()
            {
                DisplayMessage = "Welcome to the runtime. Would you like to begin?";
                Options = new[]
                {
                    new IState.Option(label: "Yes", stateInstance: State_Init.Instance),
                    new IState.Option(label: "No", stateInstance: State_Terminate.Instance)
                };
            }

            public static State_Intro Instance { get; } = new();
        }
    }
}