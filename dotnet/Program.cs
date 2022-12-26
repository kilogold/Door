// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Threading.Tasks;
using Nethereum.Web3;

namespace dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAccountBalance().Wait();
            Console.ReadLine();
        }

        static async Task GetAccountBalance()
        {
            var web3 = new Web3();
            var balance = await web3.Eth.GetBalance.SendRequestAsync("0x5fD06d66c3e02c12106d6D48E93c3447D85AD0a8");
            Console.WriteLine($"Balance in Wei: {balance.Value}");

            var etherAmount = Web3.Convert.FromWei(balance.Value);
            Console.WriteLine($"Balance in Ether: {etherAmount}");
        }
    }
}