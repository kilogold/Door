using System.Reflection.Emit;

namespace dotnet
{
    public partial class Runtime
    {
        internal interface IState
        {
            public struct Option
            {
                public string Label { get; init; }
                public IState StateInstance { get; init; }
            }

            public string DisplayMessage { get; }
            public Option[] Options { get; }

            public void Enter();
            public void Exit();

            public Task Processing();
        }

    }
}