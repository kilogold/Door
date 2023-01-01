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

            var account = new Account(privateKey);
            return new Web3(account);
        }
        
        public static async Task<Web3> ProduceWeb3FromLedgerDevice(string rpcClientUri, int chainId, Web3 fundingSource = null)
        {
            var rpcClient = new RpcClient(new Uri(rpcClientUri));
            var ledgerManagerBroker = NethereumLedgerManagerBrokerFactory.CreateWindowsHidUsb();
            var ledgerManager = (LedgerManager)await ledgerManagerBroker.WaitForFirstDeviceAsync();
            var signer = new LedgerExternalSigner(ledgerManager, 0);
            var signerAccount = new ExternalAccount(signer, chainId);
            await signerAccount.InitialiseAsync();
            signerAccount.InitialiseDefaultTransactionManager(rpcClient);

            if (fundingSource != null)
            {
                var fundingBalance = await fundingSource.Eth.GetBalance.SendRequestAsync(fundingSource.TransactionManager.Account.Address);
                var IntGas = new System.Numerics.BigInteger(50000);
                var IntGasPrice = new System.Numerics.BigInteger(50000000000);
                var IntValue = BigInteger.Subtract(fundingBalance, BigInteger.Multiply(IntGas, IntGasPrice));

                var txnInput = new TransactionInput();
                txnInput.From = fundingSource.TransactionManager.Account.Address;
                txnInput.To = signerAccount.TransactionManager.Account.Address;
                txnInput.Gas = new HexBigInteger(IntGas);
                txnInput.GasPrice = new HexBigInteger(IntGasPrice);
                txnInput.Value = new HexBigInteger(IntValue);

                await fundingSource.Eth.TransactionManager.SendTransactionAndWaitForReceiptAsync(txnInput);
            }

            return new Web3(signerAccount);
        }
    }
}