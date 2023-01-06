using Brownie.Contracts.PointOfSale_V2.ContractDefinition;

namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_PayAccess : BaseState
        {
            public static State_PayAccess Instance { get; } = new();
            private State_PayAccess() { }

            public override void Init()
            {
                DisplayMessage = "Paying for access...";
                Options = OptionTemplate_Continue(State_DoorInteraction.Instance);
            }

            public override async Task Processing()
            {
                var web3 = Blackboard.Instance.web3;
                var func = new PayForAccessFunction()
                {
                    AmountToSend = ProgramConfig.CONFIG_RATE
                };
                Console.WriteLine($"Paying: {func.AmountToSend} Wei");


                var handler = web3.Eth.GetContractTransactionHandler<PayForAccessFunction>();
                var receipt = await handler.SendRequestAndWaitForReceiptAsync(Blackboard.Instance.posContractAddress, func);
                
                Console.WriteLine("Transaction complete: " + receipt.TransactionHash);
            }
        }
    }
}