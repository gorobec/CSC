using CasinoAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Parser
    {
        public static bool IsDeposit(string record)
        {
            return record.Contains("DEPOSIT:");
        }        
        
        public static bool IsBet(string record)
        {
            return record.Contains("BET:");
        }

        public static bool IsResult(string record)
        {
            return !(IsBet(record) || IsDeposit(record));
        }

        private static DateTime GetDate(string record)
        {
            string[] parametres = record.Split(' ');
            return DateTime.ParseExact(parametres[0] + " " + parametres[1], "[dd.MM.yyyy HH:mm:ss]", null);
        }

        private static string GetName(string record)
        {
            return record.Split(' ')[2];
        }

        private static GameCode GetGameCode(string record)
        {
            return (GameCode)Enum.Parse(typeof(GameCode), record.Split(' ')[3]);
        }

        private static GameResultStatus GetGameResultStatus(string record)
        {
            return (GameResultStatus)Enum.Parse(typeof(GameResultStatus), record.Split(' ')[4]);
        }

        private static int GetBalanceChange(string record)
        {
            return Int32.Parse(record.Split(' ')[5]);
        }

        private static string GetGameInfo(string record)
        {
            var records = record.Split(' ');
            return records[6] + ' ' + records[7];
        }

        private static int GetBetValue(string record)
        {
            return Int32.Parse(record.Split(' ', ':')[4]);
        }

        public static Bet GetBet(string record)
        {
            return new Bet(GetDate(record), new User { Name = GetName(record), Balance = -1 }, GetGameCode(record), GetBetValue(record));
        }

        public static Deposit GetDeposit(string record)
        {
            return new Deposit(GetDate(record), new User { Name = GetName(record), Balance = -1 }, GetBetValue(record));
        }

        public static GameResults GetGameResults(string record)
        {
            GameResults gameResults;
            DateTime dateTime = GetDate(record);
            User user = new User { Name = GetName(record), Balance = -1 };
            GameCode gameCode = GetGameCode(record);
            GameResultStatus gameResultStatus = GetGameResultStatus(record);
            int balanceChange = GetBalanceChange(record);
            string[] gameInfo = GetGameInfo(record).Split(' ');

            switch (gameCode)
            {
                case GameCode.Dice:
                    gameResults = new DiceGameResults(dateTime, user, gameResultStatus, balanceChange, Int32.Parse(gameInfo[0]), Int32.Parse(gameInfo[1]));
                    break;
                case GameCode.Roulette:
                    gameResults = new RouletteGameResults(dateTime, user, gameResultStatus, balanceChange, gameInfo[0], Int32.Parse(gameInfo[1]));
                    break;
                default:
                    gameResults = new BlackJackGameResults(dateTime, user, gameResultStatus, balanceChange, Int32.Parse(gameInfo[0]), Int32.Parse(gameInfo[1]));
                    break;                
            }
            return gameResults;
        }
    }
}
