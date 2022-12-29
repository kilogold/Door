namespace dotnet
{
    public partial class Runtime
    {
        internal abstract class BaseState : IState
        {
            protected static IState.Option[] OptionTemplate_Continue(IState transitionState)
            {
                return new[]
                {
                    new IState.Option { Label = "Continue", StateInstance = transitionState}
                };
            }

            public string DisplayMessage { get; protected init; }
            public IState.Option[] Options { get; protected init; }

            public virtual void Enter() { /*Optional override*/ }

            public virtual void Exit() { /*Optional override*/ }

            public virtual Task Processing()
            {
                /*Optional override*/
                return Task.CompletedTask;
            }
        }
    }
}