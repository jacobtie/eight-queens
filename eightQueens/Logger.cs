using System;
using System.Text;
using System.IO;

namespace hill_climbing_eight_queens
{
    public static class Logger
    {
        private static StringBuilder _sb = new StringBuilder();

        public static void WriteLine(string line = "", bool writeConsole = true)
        {
            Write(line + "\n", writeConsole);
        }

        public static void WriteLine(char character, bool writeConsole = true)
        {
            Write(character.ToString() + "\n", writeConsole);
        }

        public static void WriteLine(int number, bool writeConsole = true)
        {
            Write(number.ToString() + "\n", writeConsole);
        }

        public static void Write(char character, bool writeConsole = true)
        {
            Write(character.ToString(), writeConsole);
        }

        public static void Write(int number, bool writeConsole = true)
        {
            Write(number.ToString(), writeConsole);
        }

        public static void Write(string line, bool writeConsole = true)
        {
            _sb.Append(line);
            if (writeConsole)
            {
                Console.Write(line);
            }
        }

        public static void Clear()
        {
            _sb.Clear();
        }

        public static void OutputToFile()
        {
            var filename = $"logs/log_{DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond}.txt";
            using (var writer = new StreamWriter(filename))
            {
                writer.Write(_sb.ToString());
            }
        }
    }
}
