# Objective
Create a door that you can pay to unlock for a period of time, like a parking meter.

# How?
EVM smart contract. Alchemy.

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

# Setup
## Dev Env
Add the following evn var to bashrc for easier iteration:
```sh
export POS_DOOR_PRIV_KEY="0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7"
```
You can use the [dev-ganashe-cli.sh](dev-ganashe-cli.sh) to spin up a dev chain with this account.