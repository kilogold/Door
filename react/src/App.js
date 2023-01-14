import { useEffect, useState } from 'react';
import { On, Off, Heading, Button, Box, Text, Stack } from './components';
import { ethers } from 'ethers';
import { loadContract, getRate, getBalance } from './contract';
import { shortenAddress } from './util';
import detectEthereumProvider from '@metamask/detect-provider'

export default function Home() {
  const [provider, setProvider] = useState();
  const [account, setAccount] = useState();
  const [balance, setBalance] = useState();
  const [chainId, setChainId] = useState();
  const [contract, setContract] = useState();
  const [error, setError] = useState("");

  const connectWallet = async (chainIdIn) => {
    try {

      const prov = await detectEthereumProvider();

      if (prov) {
        console.log('Ethereum successfully detected!')
      }
      else {
        // if the provider is not detected, detectEthereumProvider resolves to null
        throw new Error('Please install MetaMask!');
      }
      const provider = new ethers.providers.Web3Provider(prov, chainIdIn);
      setProvider(provider);

      const accounts = await provider.send("eth_requestAccounts", []);
      if (accounts) setAccount(accounts[0]);

      const rpc_chain_id = await provider.send('net_version', []);
      const rpc_chain_id_num = Number(rpc_chain_id);
      console.log("RPC Chain ID: ", rpc_chain_id_num);

      const network = await provider.getNetwork();
      setChainId(network.chainId);

      const balance = await getEthBalance(accounts[0], provider);
      setBalance(balance);

      const contract = await loadContract(provider);
      const rateEth = await getRate(contract);
      console.log("Rate: ", rateEth);
      const balEth = await getBalance(contract, provider);
      console.log("Balance: ", balEth);
      setContract(contract);

    } catch (error) {
      setError(error);
    }
  };

  const getEthBalance = async(address, lib) => {
    const balanceWei = await lib.getBalance(address);
    const balanceEth = ethers.utils.formatEther(balanceWei);
    console.log(`Balance for ${address} is: ${balanceEth}`);
    return balanceEth; 
  };

  const payBlocks = async () => {
    try {
    // The MetaMask plugin also allows signing transactions to
    // send ether and pay to change state within the blockchain.
    // For this, you need the account signer...
    console.log(provider);
    
    const signer = provider.getSigner();
    const contractWithSigner = await contract.connect(signer);
    console.log(signer);

      const payAmountEth = await getRate(contract);
      await contractWithSigner.payForAccess({value: ethers.utils.parseEther(payAmountEth)});
    } catch(e) {
      setError(e);
    }
  };

  const disconnect = async () => {
    setAccount();
    setChainId();
    setProvider();
    setBalance();
    setContract();

    console.log("disconnecting ", account);
  };

  useEffect(() => {
    console.log("Testing account connection for: ", account);


    if (provider?.on) {
      const handleDisconnect = (error) => {
        disconnect();
      };

      provider.on("disconnect", handleDisconnect);

      return () => {
        if (provider.removeListener) {
          provider.removeListener("disconnect", handleDisconnect);
        }
      };
    }
  }, [provider]);


  return (
    <>
      <Stack direction="column" justifyContent="center" height="100vh">
        <Heading>The PayWall Inc.</Heading>

        <Box>Status: {account
          ? (<On>Connected</On>)
          : (<Off>Not connected</Off>)
        }</Box>

        {account && (
          <>
            <Box>{`Network Id: ${chainId ? chainId : "none"}`}</Box>
            <Box>{`Account: ${shortenAddress(account)}`}</Box>
            <Box>{`Balance: Ξ${balance}`}</Box>
          </>
        )}

        <Box>{error ? error.message : null}</Box>

        <Box>
          {!account ? (
            <Button bg='primary' onClick={() => connectWallet(5)}>Connect Wallet</Button>
          ) : (
            <>
              <Button onClick={payBlocks}>Pay</Button>
              <br/>
              <Button onClick={disconnect}>Disconnect</Button>
            </>
          )}
        </Box>
      </Stack>
    </>
  );
}
