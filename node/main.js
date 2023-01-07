import { Alchemy, Network } from "alchemy-sdk";
import { ethers } from "ethers";
import abi from 'web3-eth-abi'

const settings = {
  apiKey: process.env.WEB3_ALCHEMY_PROJECT_ID, // Replace with your Alchemy API Key.
  network: Network.ETH_GOERLI, // Replace with your network.
};
const alchemy = new Alchemy(settings);

// This filter could also be generated with the Contract or
// Interface API. If address is not specified, any address
// matches and if topics is not specified, any log matches
const filter = {
  address: "0xb851144C34c8B7cD619d7Be2C376F8C4062A0516", //Transparent Proxy Address.
  topics: [ethers.utils.id("AccessGranted(address,uint256)")],
};

const homeAssistantURI = process.env.POS_HOME_ASSIST_URI;
const homeAssistantBearer = process.env.POS_HOME_ASSIST_TOKEN;
const homeAssistantDomain = "media_player";

function HomeAssistant_SetState(service) {
  var myHeaders = new Headers();
  myHeaders.append("Authorization", `Bearer ${homeAssistantBearer}`);
  myHeaders.append("Content-Type", "text/plain");

  var raw = "{\r\n    \"entity_id\": \"media_player.samsung_8_series_65\"\r\n}";

  var requestOptions = {
    method: 'POST',
    headers: myHeaders,
    body: raw,
    redirect: 'follow'
  };

  fetch(`${homeAssistantURI}/api/services/${homeAssistantDomain}/${service}`, requestOptions) //Ex: media_player/turn_on
    .then(response => response.text())
    .then(result => console.log(result))
    .catch(error => console.log('error', error));
}

function ParseEventLog(log) {
  var inputs = [
    {
      "indexed": false,
      "internalType": "address",
      "name": "grantee",
      "type": "address"
    },
    {
      "indexed": false,
      "internalType": "uint256",
      "name": "blockPeriod",
      "type": "uint256"
    }
  ];

  return abi.decodeLog(inputs, log.data, log.topics[0]);
}

// Subscribe to contract events.
alchemy.ws.on(filter, (log) => {
  var output = ParseEventLog(log);
  console.log(`Account ${output.grantee} has been granted access until block #${output.blockPeriod}.`);
  console.log("Turning on TV")
  HomeAssistant_SetState("turn_on");

  // Subscribe to new blocks, or newHeads
  alchemy.ws.on("block", (blockNumber) => {
    console.log(`Comparing latest block (${blockNumber}) with deadline (${output.blockPeriod})`);

    if(blockNumber.toString() === output.blockPeriod) {
      console.log("Time's up! Shutting down TV.");
      HomeAssistant_SetState("turn_off");
      alchemy.ws.off("block");
    }
  })
});

console.log("WS is locked in!")


/* 
https://docs.alchemy.com/reference/logs
EXAMPLE RESPONSE:
{
  blockNumber: 8269094,
  blockHash: "0x89ef3ec49f665faa5b2ce619be81b76a62f57cd7016d2a2afe247c62b1794110",
  transactionIndex: 25,
  removed: false,
  address: "0xb851144C34c8B7cD619d7Be2C376F8C4062A0516",
  data: "0x0000000000000000000000005fd06d66c3e02c12106d6d48e93c3447d85ad0a800000000000000000000000000000000000000000000000000000000007e2d27",
  topics: [
    "0xb4c6779ceb4a20f448e76a0e11f39bd183cff9c9dbac53df6bfcc202e2eb32f1",
  ],
  transactionHash: "0x1d85dd11b811ee2cf5ef42e60674a3179b9e4969abcee085ab807f0bc3213355",
  logIndex: 61,
}
================
NEW HEADS
  {
   "jsonrpc": "2.0",
   "method": "eth_subscription",
   "params": {
     "result": {
       "difficulty": "0x15d9223a23aa",
       "extraData": "0xd983010305844765746887676f312e342e328777696e646f7773",
       "gasLimit": "0x47e7c4",
       "gasUsed": "0x38658",
       "logsBloom": "0x00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
       "miner": "0xf8b483dba2c3b7176a3da549ad41a48bb3121069",
       "nonce": "0x084149998194cc5f",
       "number": "0x1348c9",
       "parentHash": "0x7736fab79e05dc611604d22470dadad26f56fe494421b5b333de816ce1f25701",
       "receiptRoot": "0x2fab35823ad00c7bb388595cb46652fe7886e00660a01e867824d3dceb1c8d36",
       "sha3Uncles": "0x1dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d49347",
       "stateRoot": "0xb3346685172db67de536d8765c43c31009d0eb3bd9c501c9be3229203f15f378",
       "timestamp": "0x56ffeff8",
       "transactionsRoot": "0x0167ffa60e3ebc0b080cdb95f7c0087dd6c0e61413140e39d94d3468d7c9689f"
     },
   "subscription": "0x9ce59a13059e417087c02d3236a0b1cc"
   }
 } 
    */