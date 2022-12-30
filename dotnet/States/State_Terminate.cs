namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_Terminate : BaseState
        {
            private State_Terminate()
            {
                DisplayMessage = "Shutdown";
                Options = null;
            }

            public static State_Terminate Instance { get; } = new();
        }
    }
}