using System.Diagnostics;

namespace dotnet
{
    public partial class Runtime
    {
        private IState currentState;
        public bool IsRunning { get; private set; }

        public Runtime()
        {
            IsRunning = true;
            currentState = State_Intro.Instance;
            currentState.RecursiveInit(new HashSet<IState>());
            currentState.Enter();
        }

        public void PrintStateMessage()
        {
            Console.Clear();
            Console.WriteLine(currentState.DisplayMessage);
        }

        public void PrintStateOptions()
        {
            for (int i = 0; i < currentState.Options?.Length; i++)
            {
                Console.WriteLine("[{0}] {1}", i, currentState.Options[i].Label);
            }           
        }

        private static bool InputIsQuit(ConsoleKeyInfo input)
        {
            return input.Key == ConsoleKey.Escape || input.Key == ConsoleKey.Q;
        }

        private static bool IsTerminalState(IState state)
        {
            return state.Options == null || state.Options.Length == 0;
        }
        
        private bool ShouldQuit(ConsoleKeyInfo input)
        {
            return InputIsQuit(input) || IsTerminalState(currentState);
        }
        public async Task Input()
        {
            int selectedIdx;
            while(true)
            {
                ConsoleKeyInfo input = Console.ReadKey();

                if (ShouldQuit(input))
                {
                    Shutdown();
                    return;
                }
                
                if (int.TryParse(input.KeyChar.ToString(), out selectedIdx))
                {
                    if (selectedIdx >= 0 && selectedIdx < currentState.Options.Length)
                    {
                        break;
                    }
                }
                Console.CursorLeft = Math.Max(0, Console.CursorLeft - 1);
            }
            
            await Transition(currentState.Options[selectedIdx].StateInstance);
        }

        public async Task Processing()
        {
            await currentState.Processing();
        }

        private async Task Transition(IState newState)
        {
            currentState.Exit();
            currentState = newState;
            await currentState.Enter();
        }

        private void Shutdown()
        {
            currentState.Exit();
            currentState = null;
            IsRunning = false;
        }
    }
}