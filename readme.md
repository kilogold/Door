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
    contract-->>UI: Transaction successful
end
UI->>-Door: Unlock
Bob->>+Door: Engage
Door-->>-Bob: Open
```


``` mermaid
    C4Container
    title Container diagram for Door
    Person(user, User, "A customer of the door.", $tags="v1.0")
    Person(admin, Admin, "Administrates the Door's deployment", $tags="v1.0")
    Container_Boundary(EVM, "Ethereum") {
        System(contracts, "Smart Contracts", "A suite of on-chain API's")
    }
        Container_Boundary(sysDoor, "Door") {
        Container(ui, "UI Controller", "C#", "Automates locking/unlocking mechanism")
        Container(door, "Door", "Wood", "A physical door")
    }   

    Rel(ui,door,"Locks/Unlocks")
    UpdateRelStyle(ui, door, $offsetY="-25", $offsetX="-35")
    Rel(user,ui, "Pay/validate access.")
    UpdateRelStyle(user,ui, $offsetY="75", $offsetX="120")
    Rel(user,contracts, "Pay/verify access.")
    UpdateRelStyle(user,contracts, $offsetY="75", $offsetX="-90")
    Rel(ui,contracts, "Transact/validate access.")
    UpdateRelStyle(ui,contracts, $offsetY="30", $offsetX="-65")
    Rel(user,door,"Open/Close")
    UpdateRelStyle(user,door, $offsetY="90", $offsetX="230")

    Rel(admin, contracts, "Deploy/Payout/Upgrade")
    UpdateRelStyle(admin, contracts, $offsetY="-60", $offsetX="80")
```

``` mermaid
    C4Container
    title Container diagram for administration of Smart Contracts
    Person(admin, Admin, "Administrates the Door's deployment")
    Container_Ext(ui, "UI Controller", "C#", "Automates locking/unlocking mechanism")
    Container_Boundary(EVM, "Ethereum") {
        Container(pos, "PointOfSale", "Solidity", "Logic serving as point of sale.")
        Container(proxy, "PointOfSale Proxy", "Solidity", "State serving as point of sale.")
        Container(proxyAdmin, "Proxy Admin", "Solidity", "Admin-specific operations for proxy contracts.")
    }
    Person(user, User, "A customer of the door.")

    Rel(user,ui, "Uses")
    UpdateRelStyle(user, ui, $offsetY="-10", $offsetX="0")
    Rel(admin, pos, "Deploy")
    UpdateRelStyle(admin, pos, $offsetY="80", $offsetX="-35")
    Rel(admin, proxyAdmin, "Deploy. Initiate proxy upgrade.")
    UpdateRelStyle(admin,proxyAdmin, $offsetY="70", $offsetX="180")
    Rel(admin, proxy, "Deploy")
    UpdateRelStyle(admin, proxy, $offsetY="80", $offsetX="25")
    Rel(proxyAdmin, proxy, "Upgrade")
    UpdateRelStyle(proxyAdmin, proxy, $offsetY="10", $offsetX="-35")
    Rel(proxy, pos, "Initialize. Invoke logic")
    UpdateRelStyle(proxy, pos, $offsetY="15", $offsetX="-45")
    Rel(ui,proxy,"Execute user flow")
    UpdateRelStyle(ui,proxy, $offsetY="-40", $offsetX="10")
```

# Door UI
Could be done in regular .NET app (probably best). Could also be done in Unity, but I don't need a renderer to make this happen.
To interact with an EVM network, I will need a library like Nethereum.

## Demo
### Proof Of Concept
https://user-images.githubusercontent.com/1028926/210047057-0e9c603f-eccf-4fd4-8c28-0dd55ae34af5.mp4
### Contract Deployments

| Contract      | Address     |
| -----------   | ----------- |
|PointOfSale(v1) | [0xa39BfFd1b02b3928e2FDB52FD3DAA7D8A1c875Bf](https://sepolia.etherscan.io/address/0xa39BfFd1b02b3928e2FDB52FD3DAA7D8A1c875Bf) |
|ProxyAdmin | [0x5624726dF6118BC6Ca6b17Ed40F02aFCBEFBf283](https://sepolia.etherscan.io/address/0x5624726dF6118BC6Ca6b17Ed40F02aFCBEFBf283)
|TransparentUpgradeableProxy | [0x1280071f324a0dF87E0EeE9F8Dc6729Fa0a78FDa](https://sepolia.etherscan.io/address/0x1280071f324a0dF87E0EeE9F8Dc6729Fa0a78FDa)

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
