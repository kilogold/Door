#!/usr/bin/python3
from brownie import (
    PointOfSale_V2, 
    TransparentUpgradeableProxy, 
    ProxyAdmin, 
    accounts, 
    Contract,
    run
)
from scripts.helpful_scripts import get_account, upgrade


def main(*args):
    useClef = eval(args[0])
    account = get_account(useClef)

    logic = PointOfSale_V2.deploy(
        {'from': account}
    )

    proxy = TransparentUpgradeableProxy[-1]
    proxy_admin = ProxyAdmin[-1]

    upgrade_tx = upgrade(account, proxy, logic, proxy_admin)
    upgrade_tx.wait(1)
    print("Proxy has been upgraded!")

    proxy_logic = Contract.from_abi("PointOfSale_V2", proxy.address, PointOfSale_V2.abi)

    print("Firing off new event function.")
    proxy_logic.fakeEventEmit(
        {'from': account}
    )