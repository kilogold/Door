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
            public readonly BigInteger CONFIG_RATE = Web3.Convert.ToWei(100);
            private PointOfSaleDeployment deploymentMessage;
            public static State_ContractDeploy Instance { get; } = new();

            private State_ContractDeploy(){}
            public override void Init()
            {
                deploymentMessage = new PointOfSaleDeployment()
                {
                    Rate = CONFIG_RATE // Wei
                };
                
                DisplayMessage = "Deploying contract...";
                Options = OptionTemplate_Continue(State_DoorInteraction.Instance);
            }


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