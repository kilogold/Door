namespace dotnet
{
    public interface BlockchainConnectionSettings
    {
        public string rpcClientUri { get; }
        public int chainId { get; }
        public string predefinedContractAddress { get; set; }
    }

    struct DevChainSettings : BlockchainConnectionSettings
    {
        public string rpcClientUri => "http://127.0.0.1:8545";
        public int chainId => 1337;
        public string predefinedContractAddress { get; set; }
    }

    struct SepoliaChainSettings : BlockchainConnectionSettings
    {
        public string rpcClientUri => "https://sepolia.infura.io/v3/[YOUR PROJECT ID HERE]";
        public int chainId => 11155111;
        public string predefinedContractAddress { get; set; }
    }
}
