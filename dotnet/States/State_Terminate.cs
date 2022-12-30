namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_Terminate : BaseState
        {
            public static State_Terminate Instance { get; } = new();
            private State_Terminate(){}
            public override void Init()
            {
                DisplayMessage = "Shutdown";
                Options = new IState.Option[]{};
            }

        }
    }
}