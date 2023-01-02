using System.Numerics;
using Ledger.Net;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Ledger;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace dotnet
{
    public static class Utils
    {
        public static Web3 ProduceWeb3FromEnv(string envVar)
        {
            var privateKey = Environment.GetEnvironmentVariable(envVar);
            if (string.IsNullOrEmpty(privateKey))
                throw new Exception($"Environment variable {envVar} is not defined.");

            var account = new Account(privateKey, ProgramConfig.chainId);
            return new Web3(account, ProgramConfig.rpcClientUri);
        }
        
        public static async Task<Web3> ProduceWeb3FromLedgerDevice()
        {
            var rpcClient = new RpcClient(new Uri(ProgramConfig.rpcClientUri));
            var ledgerManagerBroker = NethereumLedgerManagerBrokerFactory.CreateWindowsHidUsb();
            var ledgerManager = (LedgerManager)await ledgerManagerBroker.WaitForFirstDeviceAsync();
            var signer = new LedgerExternalSigner(ledgerManager, 0);
            var signerAccount = new ExternalAccount(signer, ProgramConfig.chainId);
            await signerAccount.InitialiseAsync();
            signerAccount.InitialiseDefaultTransactionManager(rpcClient);
            
            return new Web3(signerAccount, ProgramConfig.rpcClientUri);
        }

        public static async Task TransferFullBalance(Web3 from, Web3 to)
        {
            var fundingBalance = await from.Eth.GetBalance.SendRequestAsync(from.TransactionManager.Account.Address);
            var IntGas = new System.Numerics.BigInteger(50000);
            var IntGasPrice = new System.Numerics.BigInteger(50000000000);
            var IntValue = BigInteger.Subtract(fundingBalance, BigInteger.Multiply(IntGas, IntGasPrice));

            var txnInput = new TransactionInput();
            txnInput.From = from.TransactionManager.Account.Address;
            txnInput.To = to.TransactionManager.Account.Address;
            txnInput.Gas = new HexBigInteger(IntGas);
            txnInput.GasPrice = new HexBigInteger(IntGasPrice);
            txnInput.Value = new HexBigInteger(IntValue);

            await from.Eth.TransactionManager.SendTransactionAndWaitForReceiptAsync(txnInput);
        }
    }
}