using CasinoAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    class CasinoAnalytic : ICasinoAnalytic
    {
        // Further in code:
        // l as log, g as group, d as deposit, b as bet, res as result.

        public IEnumerable<Bet> Bets
        {
            get
            {
                return new Log()
                      .Where(l => Parser.IsBet(l))
                      .Select(l => Parser.GetBet(l));
            }
        }

        public IEnumerable<Deposit> Deposits
        {
            get
            {
                return new Log()
                      .Where(l => Parser.IsDeposit(l))
                      .Select(l => Parser.GetDeposit(l));
            }
        }

        public IEnumerable<GameResults> GamesResults
        {
            get
            {
                return new Log()
                      .Where(l => Parser.IsResult(l))
                      .Select(l => Parser.GetGameResults(l));
            }
        }

        private Bet GetMaxBet(IEnumerable<Bet> bets)
        { 
            return bets
                .Aggregate(bets.FirstOrDefault(), 
                    (max, current) => current.Value > max.Value ? current : max);
        }

        private IEnumerable<int> GetBalanceHistory(IEnumerable<GameResults> gameResults)
        {
            return gameResults
                .Select(res => gameResults.Aggregate(0, (prev, current) => prev + current.BalanceChange));
        }

        private User GetLuckyUser(IEnumerable<GameResults> gameResults)
        {
            var name = gameResults
                .GroupBy(res => res.User.Name)
                .OrderByDescending(g => g.Count(res => res.Status == GameResultStatus.Won)
                                         / (g.Count(res => res.Status == GameResultStatus.Lost) + 1))
                .FirstOrDefault().Key;

            User luckyUser = new User
            {
                Name = name,
                Balance = Deposits
                            .Where(d => d.User.Name.Equals(name))
                            .Sum(d => d.Value) +
                          gameResults
                            .Where(g => g.User.Name == name)
                            .Sum(r => r.BalanceChange)
            };
            return luckyUser;
        }
        public Bet MaxBet()
        {
            return GetMaxBet(Bets);
        }

        public Bet MaxBet(GameCode gameCode)
        {
            return GetMaxBet(Bets
                .Where(b => b.GameCode == gameCode));
        }

        public int AverageDeposit()
        {
            return (int)Deposits.Average(d => d.Value);
        }

        public int AverageDeposit(User user)
        {
            return (int)Deposits
                .Where(d => d.User.Name.Equals(user.Name))
                .Average(d => d.Value);
        }

        public IEnumerable<Deposit> TopDeposits(int count)
        {
            return Deposits
                .GroupBy(d => d.User.Name)
                .OrderByDescending(g => g.Max(d => d.Value))
                .Select(g => g.Aggregate(g.FirstOrDefault(),
                    (max, current) => current.Value > max.Value ? current : max))
                .Take(count);
        }

        public IEnumerable<User> RichestClients(int count)
        {
            return Deposits
                .GroupBy(d => d.User.Name)
                .Select(g => new { User = g.First().User, Sum = g.Sum(d => d.Value) })
                .OrderByDescending(u => u.Sum)
                .Take(count)
                .Select(u => u.User);
        }

        public GameCode MaxProfitGame(out int amount)
        {

            var maxProfitGame = GamesResults
                .GroupBy(g => g.GameCode)
                .Select(g => new {GameCode = g.First().GameCode, Sum = g.Aggregate(g.First().BalanceChange, (cur, next) => cur + next.BalanceChange)})
                .OrderBy(g => -g.Sum)
                .FirstOrDefault();

            amount = -maxProfitGame.Sum;
            return maxProfitGame.GameCode;
        }

        public User MostLuckyUser(GameCode game)
        {
            return GetLuckyUser(GamesResults.Where(res => res.GameCode == game));
        }

        public User MaxLuckyUser()
        {
            return GetLuckyUser(GamesResults);
        }

        public int UserDeposit(User user, DateTime date)
        {
            return Deposits
                .Where(d => d.User.Name.Equals(user.Name) && d.Date.ToString("dd.MM.yyyy").Equals(date.ToString("dd.MM.yyyy")))
                .Sum(d => d.Value);
        }

        public IEnumerable<int> ZeroBasedBalanceHistoryExchange(User user)
        {
            return GetBalanceHistory(GamesResults.Where(res => res.User.Name.Equals(user.Name)));
        }

        public IEnumerable<int> ZeroBasedBalanceHistoryExchange(User user, DateTime from)
        {
            return GetBalanceHistory(GamesResults.Where(res => res.User.Name.Equals(user.Name) && res.Date.CompareTo(from) >= 0));
        }

        private Dictionary<DateTime, int> GroupData(IEnumerable<GameResults> gameResults, Func<IGrouping<DateTime, GameResults>, int> function)
        {
            return gameResults
                .GroupBy(g => new DateTime(g.Date.Year, g.Date.Month, 1))
                .ToDictionary(g => g.Key, g => function(g));
        }

        public Dictionary<DateTime, int> ProfitByMonths()
        {
            return GroupData(GamesResults, g => g
                .Where(res => res.Status == GameResultStatus.Lost)
                .Sum(res => -res.BalanceChange));
        }

        public Dictionary<DateTime, int> GamesCountByMonths()
        {
            return GroupData(GamesResults, g => g.Count());
        }

        public Dictionary<DateTime, int> GamesCountByMonths(GameCode game)
        {
            return GroupData(GamesResults.Where(res => res.GameCode == game), g => g.Count());
        }

        public Dictionary<DateTime, int> NewUsersByMonths()
        {
            return GroupData(GamesResults, 
                g => g.Select(res => res.User.Name).Distinct().Count());
        }

        public Dictionary<DateTime, int> NewUsersByMonths(GameCode game)
        {
            return GroupData(GamesResults.Where(res => res.GameCode == game), 
                g => g.Select(res => res.User.Name).Distinct().Count());
        }
    }
}
