namespace dotnet
{
    public partial class Runtime
    {
        private class State_Terminate : BaseState
        {
            public State_Terminate()
            {
                DisplayMessage = "Shutdown";
                Options = null;
            }
        }
    }
}