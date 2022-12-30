#!/usr/bin/python3

from brownie import PointOfSale, accounts

def main():
    return PointOfSale.deploy(10, {'from': accounts[0]})