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

            string DisplayMessage { get; }
            Option[] Options { get; }

            void Init();
            void RecursiveInit(HashSet<IState> initProgression);
            Task Enter();
            void Exit();

            public Task Processing();
        }

    }
}