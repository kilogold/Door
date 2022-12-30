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
            return new Web3(account)
            {
                TransactionManager =
                {
                    UseLegacyAsDefault = true
                }
            };
        }
    }
}