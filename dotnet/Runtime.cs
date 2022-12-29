namespace dotnet
{
    public partial class Runtime
    {
        private IState currentState;
        public bool IsRunning { get; private set; }

        public Runtime()
        {
            IsRunning = true;
            Transition(new State_Intro());
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

        private bool InputIsQuit(ConsoleKeyInfo input)
        {
            return input.Key == ConsoleKey.Escape || input.Key == ConsoleKey.Q;
        }

        private bool IsTerminalState(IState state)
        {
            return state.Options == null || state.Options.Length == 0;
        }
        
        private bool ShouldQuit(ConsoleKeyInfo input)
        {
            return InputIsQuit(input) || IsTerminalState(currentState);
        }
        public void Input()
        {
            int selectedIdx = -1;
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
                    break;
                }
                Console.CursorLeft -= 1;
            }
            
            Transition(currentState.Options[selectedIdx].StateInstance);
        }

        public async Task Processing()
        {
            await currentState.Processing();
        }

        private void Transition(IState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        private void Shutdown()
        {
            currentState.Exit();
            currentState = null;
            IsRunning = false;
        }
    }
}