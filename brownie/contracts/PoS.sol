// SPDX-License-Identifier: MIT
pragma solidity ^0.8.17;
import "@openzeppelin/contracts/utils/math/SafeMath.sol";

contract PointOfSale {
    using SafeMath for uint256;

    mapping (address=>uint256) public blockheightDeadlines;
    uint256 public ratePerBlock;

    constructor(uint256 rate) {
        setRatePerBlock(rate);
    }

    function setRatePerBlock(uint256 amount) private {
        require(amount > 0);
        ratePerBlock = amount;
    }

    function hasAccess(address addr) public view returns (bool) {
        return blockheightDeadlines[addr] >= block.number;
    }

    function balanceOf(address _owner) public view returns (uint256) {
        uint256 deadline = blockheightDeadlines[_owner];
        return (deadline == 0) ? 0 : deadline - block.number ;
    }

    function computeBlockHeightFromPayment(uint256 payAmount) public view returns (uint256) {
        return SafeMath.div(payAmount,ratePerBlock);
    }

    function payForAccess() public payable {
        uint256 payAmount = msg.value;
        require(payAmount >= ratePerBlock, "Not enough payment for a single block.");

        uint256 newBlockAllowance = computeBlockHeightFromPayment(payAmount);
        if(hasAccess(msg.sender)) // If allowance hasn't expired...
        {
            // Extend allowance period:
            // Aggregate new blocktime with remaining balance
            blockheightDeadlines[msg.sender] += newBlockAllowance;

        }
        else // allowance expired...
        {
            // Reset allowance to a newly computed deadline.
            blockheightDeadlines[msg.sender] = block.number + newBlockAllowance;
        }
    }
}