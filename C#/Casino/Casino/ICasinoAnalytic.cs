using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;

namespace CasinoAnalysis
{
    public interface ICasinoAnalytic
    {
        /// <summary>
        /// Reuturns all bets in log
        /// </summary>
        IEnumerable<Bet> Bets { get; }
        /// <summary>
        /// Returns all deposites in log
        /// </summary>
        IEnumerable<Deposit> Deposits { get; }
        /// <summary>
        /// Reutrns all games results in lo
        /// </summary>
        IEnumerable<GameResults> GamesResults { get; }

        /// <summary>
        /// Max bet in log
        /// </summary>
        Bet MaxBet();

        /// <summary>
        /// Max bet in game
        /// </summary>
        /// <param name="code">Game code</param>
        /// <returns></returns>
        Bet MaxBet(GameCode code);

        /// <summary>
        /// Average deposite in log
        /// </summary>
        int AverageDeposit();

        /// <summary>
        /// Average deposite for user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        int AverageDeposit(User user);

        /// <summary>
        /// Maximal deposistes in log by distinct user
        /// </summary>
        /// <param name="count">Count of deposites</param>
        /// <returns></returns>
        IEnumerable<Deposit> TopDeposits(int count);

        /// <summary>
        /// Top of clients with maxmum total deposite sum
        /// </summary>
        /// <param name="count">Count of clients</param>
        /// <returns></returns>
        IEnumerable<User> RichestClients(int count);

        /// <summary>
        /// Game wich have max profit for casino
        /// </summary>
        /// <returns>GameCode of game</returns>
        GameCode MaxProfitGame(out int amount);

        /// <summary>
        /// User with count(WON)/count(LOOSE) is maximum for game
        /// </summary>
        /// <returns></returns>
        User MostLuckyUser(GameCode game);

        /// <summary>
        /// User with count(WON)/count(LOOSE) is maximum
        /// </summary>
        /// <returns></returns>
        User MaxLuckyUser();

        /// <summary>
        /// User sum of deposites on date
        /// </summary>
        /// <param name="user">User. All users have distinc names</param>
        /// <param name="date">On Date</param>
        /// <returns></returns>
        int UserDeposit(User user, DateTime date);

        /// <summary>
        /// Same as ZeroBasedBalanceHistoryExchange, but for all time
        /// </summary>
        /// <returns>Balance history</returns>
        IEnumerable<int> ZeroBasedBalanceHistoryExchange(User user);

        /// <summary>
        /// Method calc history of exchanging user balance from 0.
        /// For example:
        /// User played 3 game:
        /// First he won 100
        /// Second he loose 50
        /// And third won 150
        /// 
        /// So method returns 100, 50, 200 
        /// </summary>
        /// <param name="from">Start date</param>
        /// <returns></returns>
        IEnumerable<int> ZeroBasedBalanceHistoryExchange(User user, DateTime from);

        /// <summary>
        /// Month's profit ordered by first date of month
        /// </summary>
        /// <returns></returns>
        Dictionary<DateTime, int> ProfitByMonths();

        /// <summary>
        /// Games played by months
        /// </summary>
        /// <returns></returns>
        Dictionary<DateTime, int> GamesCountByMonths();

        /// <summary>
        /// Special games played by months
        /// </summary>
        /// <returns></returns>
        Dictionary<DateTime, int> GamesCountByMonths(GameCode game);

        /// <summary>
        /// New user by months
        /// </summary>
        /// <returns>Return number of user with start plays in month</returns>
        Dictionary<DateTime, int> NewUsersByMonths();

        /// <summary>
        /// New user in game by months
        /// </summary>
        /// <returns>Return number of user with start plays in month</returns>
        Dictionary<DateTime, int> NewUsersByMonths(GameCode game);
    }

    public class Bet
    {
        public Bet(DateTime date, User user, GameCode gameCode, int value)
        {
            Date = date;
            User = user;
            GameCode = gameCode;
            Value = value;
        }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public GameCode GameCode { get; set; }
        public int Value { get; set; }
    }

    public class Deposit
    {
        public Deposit(DateTime date, User user, int value)
        {
            Date = date;
            User = user;
            Value = value;
        }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public int Value { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public int Balance { get; set; }
    }

    public enum GameResultStatus
    {
        Lost,
        Won
    }

    public enum GameCode
    {
        BJ,
        Roulette,
        Dice
    }

    public abstract class GameResults
    {
        public DateTime Date { get; private set; }
        public User User { get; private set; }
        public GameResultStatus Status { get; private set; }
        public int BalanceChange { get; private set; }

        public abstract GameCode GameCode { get; }

        protected GameResults(DateTime date, User user, GameResultStatus status, int balanceChange)
        {
            Date = date;
            User = user;
            Status = status;
            BalanceChange = balanceChange;
        }
    }

    public class BlackJackGameResults : GameResults
    {

        public BlackJackGameResults(DateTime date, User user, GameResultStatus status, int balanceChange, int userScore, int casinoScore)
            : base(date, user, status, balanceChange)
        {
            UserScore = userScore;
            CasinoScore = casinoScore;
        }

        public override GameCode GameCode
        {
            get { return GameCode.BJ; }
        }

        public int CasinoScore { get; private set; }
        public int UserScore { get; private set; }
    }

    public class RouletteGameResults : GameResults
    {
        public RouletteGameResults(DateTime date, User user, GameResultStatus status, int balanceChange, string userChoise, int rouletCell)
            : base(date, user, status, balanceChange)
        {
            UserChoise = userChoise;
            RouletteCell = rouletCell;
        }

        public override GameCode GameCode
        {
            get { return GameCode.Roulette; }
        }

        public int RouletteCell { get; private set; }
        public string RouletteCellColor { get { return RouletteCell % 2 == 0 ? "red" : "black"; } }
        public string UserChoise { get; private set; }
        public int? UserChoiseCell
        {
            get
            {
                int userChoiseCell;
                if (int.TryParse(UserChoise, out userChoiseCell))
                {
                    return userChoiseCell;
                }
                return null;
            }
        }
        public string UserChoiseCellColor { get { return UserChoise; } }
    }

    public class DiceGameResults : GameResults
    {
        public DiceGameResults(DateTime date, User user, GameResultStatus status, int balanceChange, int userScore, int casinoScore)
            : base(date, user, status, balanceChange)
        {
            UserScore = userScore;
            CasinoScore = casinoScore;
        }

        public override GameCode GameCode
        {
            get { return GameCode.Dice; }
        }

        public int CasinoScore { get; private set; }
        public int UserScore { get; private set; }
    }
}