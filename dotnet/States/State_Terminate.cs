namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_Terminate : BaseState
        {
            private State_Terminate()
            {
                DisplayMessage = "Shutdown";
                Options = new IState.Option[]{};
            }

            public static State_Terminate Instance { get; } = new();
        }
    }
}