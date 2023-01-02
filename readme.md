# Objective
Create a door that you can pay to unlock for a period of time, like a parking meter.

# How?
EVM smart contract.

``` mermaid
sequenceDiagram

actor Bob
participant Door as Door
participant UI as Door UI
participant contract as EVM Smart Contract

Bob->>+Door: Engage
Door--x-Bob: Locked
Bob->>+UI: Present wallet address
UI->>contract: Check allowance for wallet address
alt Current permision
    contract-->>UI: "Valid allowance" 
else Not allowed or permission expired
    contract-->>UI: "Invalid allowance" 
    UI->>contract: GetPrice(void):uint256
    contract-->>UI: price per block
    UI-->>-Bob: transaction prompt
    Bob->>+UI: Sign transaction
    UI->>contract: Register sender allowance[Payable]
    contract->>contract: Compute unlock (block) time
    Alchemy-->>UI: Transaction successful
end
UI->>-Door: Unlock
Bob->>+Door: Engage
Door-->>-Bob: Open
```


``` mermaid
    C4Container
    title Container diagram for Door
    Person(user, User, "A customer of the door.", $tags="v1.0")
    Container_Boundary(EVM, "Ethereum") {
        Container(pos, "PoS", "Solidity", "Smart contract serving as point of sale.")
    }
    Container_Boundary(sysDoor, "Door") {
        Container(ui, "UI Controller", "C#", "Automates locking/unlocking mechanism")
        Container(door, "Door", "Wood", "A physical door")
    }
    Rel(ui,door,"Locks/Unlocks")
    UpdateRelStyle(ui, door, $offsetY="-25", $offsetX="-35")
    Rel(user,ui, "Pay/validate access.")
    UpdateRelStyle(user,ui, $offsetY="75", $offsetX="120")
    Rel(user,pos, "Pay/verify access.")
    UpdateRelStyle(user,pos, $offsetY="75", $offsetX="-80")
    Rel(ui,pos, "Transact/validate access.")
    UpdateRelStyle(ui,pos, $offsetY="-25", $offsetX="-65")
    Rel(user,door,"Open/Close")
    UpdateRelStyle(user,door, $offsetY="90", $offsetX="230")
```

# Door UI
Could be done in regular .NET app (probably best). Could also be done in Unity, but I don't need a renderer to make this happen.
To interact with an EVM network, I will need a library like Nethereum.

## Demo
### Proof Of Concept
https://user-images.githubusercontent.com/1028926/210047057-0e9c603f-eccf-4fd4-8c28-0dd55ae34af5.mp4
### Contract Deployment
Sepolia testnet: https://sepolia.etherscan.io/address/0x7bc3579c9d0ed872deb7f9515dbf1c7235ee8bdc#code

# Setup
## Required Tools
* Ganache v7.7.2
* Brownie v1.19.2
* .NET 7 SDK
## Additional Recommended Tools
* VSCode + Solidity plugin (generates C# wrappers)

## Dev Env
Add the following env vars to for operation:
| Variable      | Description |
| -----------   | ----------- |
| POS_USER_PRIV_KEY             | Any private key representing an **embedded user account** for payment transactions    |
| POS_ADMIN_PRIV_KEY            | Any private key representing the **deployer/owner account** of the smart contract     |
| POS_LEDGER_FUNDING_PRIV_KEY   | Amply funded account used to optionally transfer funds onto a Ledger-provided account |

You can use the [dev-ganache-cli.sh](dev-ganache-cli.sh) or [dev-ganache-cli.bat](dev-ganache-cli.bat) to spin up an appropriate dev chain with required accounts.
> **_NOTE:_**  Ledger device user flows are **Windows only**.

# Project Structure
This is a single repository housing a .NET & Brownie project, each in their respective root directories.
To take advantage of IDE configurations, ensure you open the project within their root scopes:
* [brownie](./brownie) for VSCode
* [dotnet](./dotnet) for Rider
