using System.Runtime.InteropServices;

namespace dotnet
{
    public partial class Runtime
    {
        private abstract class BaseStateVariable<T> : BaseState
        {
            protected Dictionary<T, Tuple<string, IState.Option[]>> alternatives;

            protected Func<T> ObservedVariable;

            public override string DisplayMessage => alternatives[ObservedVariable()].Item1;
            public override IState.Option[] Options => alternatives[ObservedVariable()].Item2;
            
            public override void RecursiveInit(HashSet<IState> initProgression)
            {
                if (initProgression.Contains(this))
                {
                    return;
                }
                
                Init();
                initProgression.Add(this);

                foreach (var alt in alternatives)
                {
                    foreach (var opt in alt.Value.Item2)
                    {
                        opt.StateInstance.RecursiveInit(initProgression);
                    }                    
                }
            }

        }
    }
}