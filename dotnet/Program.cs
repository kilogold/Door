namespace dotnet
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var runtime = new Runtime();
            while (runtime.IsRunning)
            {
                runtime.PrintStateMessage();
                await runtime.Processing();
                runtime.PrintStateOptions();
                await runtime.Input();
            }
            return 0;
        }
    }
}