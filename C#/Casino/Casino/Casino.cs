using CasinoAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    class Casino
    {
        private const int MAXSUM = 10000000;
        private Dictionary<string, int> Balance = new Dictionary<string,int>();
        private string UserInfo = @"users.dat";
        public Logger FileLogger; 
        private void ProcessClientInfo()
        { 
            if (File.Exists(UserInfo))
            {
                var userInfo = File.ReadAllLines(UserInfo);
                foreach(string line in userInfo)
                {
                    var lines = line.Split(' ');
                    Balance[lines[0]] = Int32.Parse(lines[1]);
                }
            }
        }

        public void PutMoney(Player player)
        { 
                int sum = SafeInput.InputNumberInRange(1, MAXSUM);
                player.PutMoney(sum);
                Balance[player.Name] += sum;
                Deposit deposit = new Deposit(DateTime.UtcNow, player.CasinoAccount, sum);
                FileLogger.RecordDeposit(deposit);
                Console.WriteLine("{0} rubles successfully added.", sum);
        }

        public void Open()
        {
            FileLogger = new Logger();
            FileLogger.Initialize(null);
            ProcessClientInfo();
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();
            if (!Balance.ContainsKey(name))
            { Balance[name] = 0; }
            Player player = new Player(name, Balance[name]);
            Console.WriteLine("{0}, your balance is {1} rubles. Want to add some money? (Y/N)", player.Name, player.Balance);
            string answer = Console.ReadLine();
            if (answer == "Y")
            {
                PutMoney(player);
            }
            Console.WriteLine("Now you can start playing.");
            string gameType = "";
            bool tryAgain = false;
            while (gameType != "exit")
            {
                if (!tryAgain)
                {
                    Console.WriteLine("Please choose the game: dice, roulette or blackjack. To exit enter 'exit'.");
                    gameType = Console.ReadLine();
                    if (gameType == "exit")
                    {
                        Close();
                        return;
                    }
                }
                tryAgain = ArrangeGame(gameType, player);
            }
        }

        public bool ArrangeGame(string gameType, Player player)
        {
            Game game;
            if (!(gameType == "dice" || gameType == "roulette" || gameType == "blackjack"))
            {
                Console.WriteLine("You can not play {0} here.", gameType);
                return false;
            }

            int sum = 0;
            do
            {
                Console.WriteLine("Are you sure that you want to play {0}? (Y/N) You can not quit the game while playing!", gameType);
                string answer = Console.ReadLine();
                if (answer == "N")
                { return false; }

                Console.WriteLine("Place your bet.");
                try
                { sum = Int32.Parse(Console.ReadLine()); }
                catch (FormatException)
                {
                    Console.WriteLine("Number is incorrect.");
                    sum = -1;
                    continue;
                }

                if (player.Balance < sum)
                {
                    Console.WriteLine("Not enough money for the bet. Want to put more money on your Balance?(Y/N)");
                    answer = Console.ReadLine();
                    if (answer == "Y")
                    { PutMoney(player); }
                }
                if (sum < 0)
                {
                    Console.WriteLine("You can not bet negative number.");
                    continue;
                }
            } while (player.Balance < sum || sum < 0);
            Bet bet; 
            

            if (gameType == "dice")
            { 
                game = new Dice(player, sum); 
                bet = new Bet(DateTime.UtcNow, player.CasinoAccount, GameCode.Dice, sum);
            }
            else if (gameType == "roulette")
            {
                game = new Roulette(player, sum);
                bet = new Bet(DateTime.UtcNow, player.CasinoAccount, GameCode.Roulette, sum);
            }
            else
            {
                game = new BlackJack(player, sum);
                bet = new Bet(DateTime.UtcNow, player.CasinoAccount, GameCode.BJ, sum);
            }
            FileLogger.RecordBet(bet);

            GameResults result = player.Play(game);
            FileLogger.RecordResult(result);
            player.PutMoney(result.BalanceChange);
            Balance[player.Name] += result.BalanceChange;
            player.Result = 0;
            if (result.Status == GameResultStatus.Lost)
            {
                Console.WriteLine("You lost! Want to try again?(Y/N)");
            }
            else 
            {
                Console.WriteLine("Congratulations! You won! Want to try again?(Y/N)");
            }
            string tryAgain = Console.ReadLine();
            return (tryAgain == "Y" ? true : false);
        }
        private void Close()
        {
            FileLogger.Shutdown();
            StreamWriter streamWriter = new StreamWriter(UserInfo);
            foreach (string name in Balance.Keys)
            {
                streamWriter.WriteLine(name + " " + Balance[name]);
            }
            streamWriter.Close();
        }
    }
}
