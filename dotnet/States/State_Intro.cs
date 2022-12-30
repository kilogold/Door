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
                    new IState.Option { Label = "Yes", StateInstance = State_Init.Instance },
                    new IState.Option { Label = "No", StateInstance = State_Terminate.Instance }
                };
            }

            public static State_Intro Instance { get; } = new();
        }
    }
}