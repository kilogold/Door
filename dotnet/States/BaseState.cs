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
                    new IState.Option("Continue", transitionState)
                };
            }

            public virtual string DisplayMessage { get; protected init; }
            public virtual IState.Option[] Options { get; protected init; }
            public virtual void Exit() { /*Optional override*/ }
            public virtual Task Enter()
            {
                return Task.CompletedTask;
                /*Optional override*/
            }
            public virtual Task Processing()
            {
                /*Optional override*/
                return Task.CompletedTask;
            }
        }
    }
}