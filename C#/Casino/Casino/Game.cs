using CasinoAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    abstract class Game
    {
        protected Player player;
        protected int bet;
        protected Game(Player player, int bet)
        { 
            this.player = player;
            this.bet = bet;
        }
        abstract public GameResults Play();
    }
    class Dice : Game 
    {
        public Dice(Player player, int bet) : base(player, bet) { }
        public override GameResults Play()
        { 
            int number = SafeInput.InputNumberInRange(2, 12);

            Random rand = new Random();
            int firstDiceValue = rand.Next(1, 7);
            int secondDiceValue = rand.Next(1, 7);
            Console.WriteLine("Dices showed {0} and {1}, with sum {2}", firstDiceValue, secondDiceValue, firstDiceValue + secondDiceValue);

            if (firstDiceValue + secondDiceValue == number)
            { return new DiceGameResults(DateTime.UtcNow, player.CasinoAccount, GameResultStatus.Won, bet, number, firstDiceValue + secondDiceValue); }
            return new DiceGameResults(DateTime.UtcNow, player.CasinoAccount, GameResultStatus.Lost, -bet, number, firstDiceValue + secondDiceValue);
        }
    }

    class Roulette : Game
    {
        public Roulette(Player player, int bet) : base(player, bet) { }
        public override GameResults Play()
        {
            Random rand = new Random();
            string answer = "";
            string choice = "";
            int rouletteCell = -1;

            while (!(answer == "color" || answer == "cell"))
            {
                Console.WriteLine("What do you want to bet on, color or cell?");
                answer = Console.ReadLine();
                rouletteCell = rand.Next(1, 37);
                if (answer == "color")
                {
                    Console.WriteLine("Choose the color (red/black):");
                    choice = Console.ReadLine();
                    string rightColor = (rouletteCell % 2 == 0 ? "red" : "black");
                    Console.WriteLine("Correct color is {0}, in the cell {1}", rightColor, rouletteCell);
                    if (rightColor == choice)
                    { return new RouletteGameResults(DateTime.UtcNow, player.CasinoAccount, GameResultStatus.Won, bet, choice, rouletteCell); }
                }
                else if (answer == "cell")
                {
                    int cell = SafeInput.InputNumberInRange(1, 36);
                    choice = cell.ToString();
                    Console.WriteLine("Correct cell is " + rouletteCell);
                    if (rouletteCell == cell)
                    { return new RouletteGameResults(DateTime.UtcNow, player.CasinoAccount, GameResultStatus.Won, 3 * bet, choice, rouletteCell); }
                }
            }
            return new RouletteGameResults(DateTime.UtcNow, player.CasinoAccount, GameResultStatus.Lost, -bet, choice, rouletteCell);
        }
    }

    class BlackJack : Game
    {
        Dictionary<int, string> deck = new Dictionary<int,string>();
        Random rand = new Random();

        public BlackJack(Player player, int bet)
            : base(player, bet)
        {
            for (int i = 2; i < 11; ++i)
            {
                deck[i] = i.ToString();
            }
            deck[11] = "Ace";
            deck[12] = "Jack";
            deck[13] = "Queen";
            deck[14] = "King";
        }
        class Dealer
        {
            public int sum;
        }
        int chooseCard()
        { 
            int newCard = rand.Next(2, 15);
            player.Result += (newCard < 12 ? newCard : 10);
            return newCard;
        }
        public override GameResults Play()
        {
            Dealer opponent = new Dealer();
            List<int> hand = new List<int>();

            while (opponent.sum <= 21)
            {
                int newCard = rand.Next(2, 15);
                newCard = (newCard < 12 ? newCard : 10);
                if (opponent.sum + newCard > 21)
                { break; }
                opponent.sum += newCard;
            }

            hand.Add(chooseCard());
            hand.Add(chooseCard());
            string answer = "";
            do
            {
                Console.Write("You have {0} cards: ", hand.Count());
                foreach (int weight in hand)
                { 
                    Console.Write("{0}, ", deck[weight]);
                }
                Console.WriteLine("Your sum is {0}. Would you like one more card?(Y/N)", player.Result);      
                answer = Console.ReadLine();
                if (answer == "N")
                { break; }
                hand.Add(chooseCard());
                if (player.Result > 21)
                {
                    Console.WriteLine("Your sum is {0}, that is more than 21. You lost.", player.Result);
                    return new BlackJackGameResults(DateTime.UtcNow, player.CasinoAccount, GameResultStatus.Lost, -bet, player.Result, opponent.sum);
                }
            } while (answer == "Y");

            Console.WriteLine("Opponent's sum is " + opponent.sum.ToString());
            return new BlackJackGameResults(DateTime.UtcNow, player.CasinoAccount, (opponent.sum >= player.Result ? GameResultStatus.Lost : GameResultStatus.Won),
                (opponent.sum >= player.Result ? -bet : bet), player.Result, opponent.sum);
        }
    }
}
