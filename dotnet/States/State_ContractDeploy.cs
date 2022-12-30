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
            private const int CONFIG_RATE = 10;
            
            private readonly PointOfSaleDeployment deploymentMessage;

            private State_ContractDeploy()
            {
                deploymentMessage = new PointOfSaleDeployment()
                {
                    Rate = CONFIG_RATE // Wei
                };
                
                DisplayMessage = "Deploying contract...";
                Options = OptionTemplate_Continue(State_DoorInteraction.Instance);
            }

            public static State_ContractDeploy Instance { get; } = new();

            public override async Task Processing()
            {
                var output = await PointOfSaleService.DeployContractAndWaitForReceiptAsync(
                    Blackboard.Instance.web3, 
                    deploymentMessage);
                
                Blackboard.Instance.posContractAddress = output.ContractAddress;
                Console.WriteLine($"New contract deployed at: {output.ContractAddress}");
            }
        }
    }
}