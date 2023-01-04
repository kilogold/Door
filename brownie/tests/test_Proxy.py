#!/usr/bin/python3
import brownie
import pytest

from brownie import run

def test_deployment():
    run("pos_deploy", args=("False",))
    run("pos_upgrade", args=("False",))
