using System.Numerics;
using Brownie.Contracts.PointOfSale;
using Brownie.Contracts.PointOfSale.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace dotnet
{
    public partial class Runtime
    {
        private class State_ContractDeploy : BaseState
        {
            private readonly PointOfSaleDeployment deploymentMessage;
            
            public State_ContractDeploy(BigInteger rate)
            {
                deploymentMessage = new PointOfSaleDeployment()
                {
                    Rate = rate // Wei
                };
                
                DisplayMessage = "Deploying contract...";
                Options = OptionTemplate_Continue(new State_Terminate());
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