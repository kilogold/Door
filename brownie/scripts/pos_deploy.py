#!/usr/bin/python3

from brownie import (
    PointOfSale, 
    TransparentUpgradeableProxy, 
    ProxyAdmin, 
    accounts, 
    Contract, 
    Wei
)

from scripts.helpful_scripts import encode_function_data, get_account

def main(*args):
    useClef = eval(args[0])
    account = get_account(useClef)
    
    logic_contract = PointOfSale.deploy(
        {'from': account})

    proxy_admin_contract = ProxyAdmin.deploy(
        {"from": account},
    )

    proxy_contract = TransparentUpgradeableProxy.deploy(
        logic_contract.address,
        proxy_admin_contract.address,
        encode_function_data(logic_contract.initialize, Wei("0.001 ether")),
        {"from": account, "gas_limit": 1000000},
    )    
    print(f"Proxy deployed to {proxy_contract}. You can now upgrade it.")


    proxy_logic = Contract.from_abi("PointOfSale", proxy_contract.address, PointOfSale.abi)
    print(f"Here is the initial rate value: {proxy_logic.ratePerBlock()}")

