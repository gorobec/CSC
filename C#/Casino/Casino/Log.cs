using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    class Log : IEnumerable<string>
    {
        private string FileName = @"log.dat";

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new LogEnumerator(FileName);
        }

        public class LogEnumerator : IEnumerator<string>
        {
            private StreamReader streamReader;
            private string line;
            public LogEnumerator(string fileName)
            {
                streamReader = new StreamReader(fileName);
            }

            public bool MoveNext()
            {
                string newLine = streamReader.ReadLine();

                if (newLine != null)
                {
                    line = newLine;
                    return true;
                }
                else
                { return false; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public string Current
            {
                get { return line; }
            }

            void IDisposable.Dispose()
            {
                streamReader.Dispose();
            }
            public void Reset()
            { }
        }
    }
}
