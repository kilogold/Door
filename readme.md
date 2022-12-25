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