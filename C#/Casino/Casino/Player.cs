using CasinoAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    class Player
    {
        public string Name { get; private set; }
        public int Balance { get; set; }
        public int Result { get; set; }
        public User CasinoAccount { get; private set; }
        public Player(string name, int balance)
        {
            this.Name = name;
            this.Balance = balance;
            this.Result = 0;
            CasinoAccount = new User();
            CasinoAccount.Name = this.Name;
            CasinoAccount.Balance = this.Balance;
        }
        public void PutMoney(int sum)
        {
            this.Balance += sum;
            CasinoAccount.Balance = Balance;
        }
        public GameResults Play(Game game)
        {
            return game.Play();
        }
    }
}
