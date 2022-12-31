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
            private PointOfSaleDeployment deploymentMessage;
            public static State_ContractDeploy Instance { get; } = new();

            private State_ContractDeploy(){}
            public override void Init()
            {
                deploymentMessage = new PointOfSaleDeployment()
                {
                    Rate = ProgramConfig.CONFIG_RATE
                };
                
                DisplayMessage = "Deploying contract...";
                Options = OptionTemplate_Continue(State_DoorInteraction.Instance);
            }


            public override async Task Processing()
            {
                var web3 = Utils.ProduceWeb3FromEnv("POS_ADMIN_PRIV_KEY");
                var output = await PointOfSaleService.DeployContractAndWaitForReceiptAsync(
                    web3, deploymentMessage);
                
                Blackboard.Instance.posContractAddress = output.ContractAddress;
                Console.WriteLine($"New contract deployed at: {output.ContractAddress}");
            }
        }
    }
}