namespace dotnet
{
    public partial class Runtime
    {
        private class State_Intro : BaseState
        {
            public State_Intro()
            {
                DisplayMessage = "Welcome to the runtime. Would you like to begin?";
                Options = new[]
                {
                    new IState.Option { Label = "Yes", StateInstance = new State_Init() },
                    new IState.Option { Label = "No", StateInstance = new State_Terminate() }
                };
            }
        }
    }
}