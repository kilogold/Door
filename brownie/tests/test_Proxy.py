#!/usr/bin/python3
import brownie
import pytest

from brownie import run

def test_deployment():
    run("pos_deploy")
    run("pos_upgrade")
