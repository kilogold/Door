// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Brownie.Contracts.PointOfSale;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Brownie.Contracts.PointOfSale.ContractDefinition;
using Brownie.Contracts.Token;
using Brownie.Contracts.Token.ContractDefinition;

namespace dotnet
{
    class Program
    {
        private const string KEY_ENV_VAR = "POS_DOOR_PRIV_KEY";
        static async Task<int> Main(string[] args)
        {
            var privateKey = Environment.GetEnvironmentVariable(KEY_ENV_VAR);
            if (string.IsNullOrEmpty(privateKey))
                throw new Exception($"Environment variable {KEY_ENV_VAR} is not defined.");
            
            var account = new Account(privateKey);
            var web3 = new Web3(account);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var receipt = await DeployPOSContract(web3);
            await ReadPOSContract(web3,receipt.ContractAddress);
            return 0;
        }

        static async Task ReadPOSContract(Web3 web3, string contractAddress)
        {
            var func = new HasAccessFunction {
                Addr = web3.TransactionManager.Account.Address
            };
            
            var handler = web3.Eth.GetContractQueryHandler<HasAccessFunction>();
            var hasAccess = await handler.QueryAsync<bool>(contractAddress, func);
            
            Console.WriteLine($"Contract access for {func.Addr}: {hasAccess}");
        }
        
        static async Task<TransactionReceipt> DeployPOSContract(Web3 web3)
        {
            var deploymentMessage = new PointOfSaleDeployment()
            {
                Rate = 10 // Wei
            };

            var receipt = await PointOfSaleService.DeployContractAndWaitForReceiptAsync(web3, deploymentMessage);
            Console.WriteLine($"New contract deployed at: {receipt.ContractAddress}");
            return receipt;
        }

        static async Task GetAccountBalance()
        {
            var web3 = new Web3();
            var balance = await web3.Eth.GetBalance.SendRequestAsync("0x5fD06d66c3e02c12106d6D48E93c3447D85AD0a8");
            Console.WriteLine($"Balance in Wei: {balance.Value}");

            var etherAmount = Web3.Convert.FromWei(balance.Value);
            Console.WriteLine($"Balance in Ether: {etherAmount}");
        }
    }
}