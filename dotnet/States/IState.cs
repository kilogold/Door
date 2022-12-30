using System.Diagnostics;
using System.Reflection.Emit;

namespace dotnet
{
    public partial class Runtime
    {
        internal interface IState
        {
            public struct Option
            {
                public Option(string label, IState stateInstance)
                {
                    Label = label;
                    StateInstance = stateInstance;
                }

                public string Label { get; }
                public IState StateInstance { get; }
            }

            public string DisplayMessage { get; }
            public Option[] Options { get; }

            public Task Enter();
            public void Exit();

            public Task Processing();
        }

    }
}