#!/usr/bin/python3

from brownie import PointOfSale, accounts

def main():
    return PointOfSale.deploy(10, {'from': accounts[0]})

#$ ganache-cli  --hardfork istanbul --account="0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7",100000000000000000000000000
