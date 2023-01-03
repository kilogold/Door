using System.Numerics;
using Brownie.Contracts.PointOfSale;
using Brownie.Contracts.PointOfSale.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace dotnet
{
    public partial class Runtime
    {
        private sealed class State_ContractDeploy : BaseState
        {
            public static State_ContractDeploy Instance { get; } = new();

            private State_ContractDeploy(){}
            public override void Init()
            {

                DisplayMessage = "Deploying contract...";
                Options = OptionTemplate_Continue(State_DoorInteraction.Instance);
            }


            public override async Task Processing()
            {
                var deploymentMessage = new PointOfSaleDeployment()
                {
                };
                                
                var output = await PointOfSaleService.DeployContractAndWaitForReceiptAsync(
                    Blackboard.Instance.web3, deploymentMessage);
                
                Blackboard.Instance.posContractAddress = output.ContractAddress;
                Console.WriteLine($"New contract deployed at: {output.ContractAddress}");

                var func = new InitializeFunction()
                {
                    Rate = ProgramConfig.CONFIG_RATE
                };
                var handler = Blackboard.Instance.web3.Eth.GetContractTransactionHandler<InitializeFunction>();
                await handler.SendRequestAndWaitForReceiptAsync(Blackboard.Instance.posContractAddress, func);
                Console.WriteLine($"Contract initialized with rate of: {ProgramConfig.CONFIG_RATE}");

            }
        }
    }
}