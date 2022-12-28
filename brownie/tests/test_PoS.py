#!/usr/bin/python3
import brownie
import pytest

@pytest.fixture(scope="module")
def pos(PointOfSale, accounts):
    return PointOfSale.deploy(10, {'from': accounts[0]})

def test_initial_balance(accounts, pos):
    user = accounts[0]
    assert pos.balanceOf(user) == 0
    assert pos.hasAccess(user) == False
    assert pos.balance() == 0

def test_pay_for_access_fail_balance(accounts, pos):
    user = accounts[0]
    invalid_amount = pos.ratePerBlock()/2
    with brownie.reverts():
        pos.payForAccess({'value': invalid_amount})

def test_pay_for_access_successful_balance(accounts, pos):
    user = accounts[0]
    assert pos.hasAccess(user) == False

    three_block_allowance = pos.ratePerBlock() * 3
    pos.payForAccess({'value': three_block_allowance})
    assert pos.hasAccess(user) == True
    assert pos.balance() > 0

