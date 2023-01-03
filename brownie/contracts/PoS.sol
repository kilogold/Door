// SPDX-License-Identifier: MIT
pragma solidity ^0.8.17;
import "@upgradable-openzeppelin/contracts/utils/math/SafeMathUpgradeable.sol";
import "@upgradable-openzeppelin/contracts/access/OwnableUpgradeable.sol";

contract PointOfSale is Initializable, OwnableUpgradeable {
    using SafeMathUpgradeable for uint256;

    mapping (address=>uint256) public blockheightDeadlines;
    uint256 public ratePerBlock;
    
    function initialize(uint256 rate) public initializer {
        __Context_init_unchained();
        __Ownable_init_unchained();
        setRatePerBlock(rate);
    }

    function payOut() public onlyOwner {
        payable(owner()).transfer(address(this).balance);
    }

    function setRatePerBlock(uint256 amount) public onlyOwner {
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
        return SafeMathUpgradeable.div(payAmount,ratePerBlock);
    }

    function payForAccess() public payable {
        uint256 payAmount = msg.value;
        require(payAmount >= ratePerBlock, "Not enough payment for a single block.");

        uint256 newBlockAllowance = computeBlockHeightFromPayment(payAmount);
        if(hasAccess(_msgSender())) // If allowance hasn't expired...
        {
            // Extend allowance period:
            // Aggregate new blocktime with remaining balance
            blockheightDeadlines[_msgSender()] += newBlockAllowance;

        }
        else // allowance expired...
        {
            // Reset allowance to a newly computed deadline.
            blockheightDeadlines[_msgSender()] = block.number + newBlockAllowance;
        }
    }
}