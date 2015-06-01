using CasinoAnalysis;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    class Logger : ILogger
    {
        private StreamWriter streamWriter;
        private string logFile = @"log.dat";

        public void Initialize(IEventSource eventSource)
        {
            streamWriter = new StreamWriter(logFile, true);
        }

        public void RecordDeposit(Deposit deposit)
        {
            streamWriter.WriteLine("[{0}] {1} DEPOSIT:{2}", deposit.Date.ToString("dd.MM.yyyy HH:mm:ss"), deposit.User.Name, deposit.Value);
        }
        public void RecordBet(Bet bet)
        {
            streamWriter.WriteLine("[{0}] {1} BET:{2}", bet.Date.ToString("dd.MM.yyyy HH:mm:ss"), bet.User.Name, bet.Value);
        }
        public void RecordResult(GameResults result)
        {

            streamWriter.Write("[{0}] {1} {2} {3} {4} ", result.Date.ToString("dd.MM.yyyy HH:mm:ss"), result.User.Name, result.GameCode, result.Status.ToString(), result.BalanceChange);
            if (result.GameCode == GameCode.BJ)
            {
                streamWriter.Write("{0} {1}", ((BlackJackGameResults)result).UserScore, ((BlackJackGameResults)result).CasinoScore);
            }
            else if (result.GameCode == GameCode.Roulette)
            {
                streamWriter.Write("{0} {1}", ((RouletteGameResults)result).UserChoiseCellColor, ((RouletteGameResults)result).RouletteCell);
            }
            else if (result.GameCode == GameCode.Dice)
            {
                streamWriter.Write("{0} {1}", ((DiceGameResults)result).UserScore, ((DiceGameResults)result).CasinoScore);
            }
            streamWriter.WriteLine();
           
        }

        public void Shutdown()
        {
            streamWriter.Close();
        }
        public string Parameters { get; set; }
        public LoggerVerbosity Verbosity { get; set; }
    }
}
