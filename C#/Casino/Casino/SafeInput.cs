using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    class SafeInput
    {
        public static int InputNumberInRange(int from, int to)
        {
            int result = -1;
            while (!(result >= from && result <= to))
            {
                Console.WriteLine("Choose the number from {0} to {1}.", from, to);
                try
                { result = Int32.Parse(Console.ReadLine()); }
                catch (FormatException)
                {
                    Console.WriteLine("Number is incorrect.");
                    result = -1;
                }
            }
            return result;
        }

    }
}
