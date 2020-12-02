using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode2020
{
    public class Day2
    {
        public class TobogganPassword
        {
            Regex _rx = new Regex(@"(\d+)-(\d+)\s+(\w):\s(\w+)");

            private int minTimes = 0;
            private int maxTimes = 0;
            private char charRequired;
            private string password;

            public TobogganPassword(int min, int max, char c, string p)
            {
                this.minTimes = min;
                this.maxTimes = max;
                this.charRequired = c;
                this.password = p;
            }

            public TobogganPassword(string fullInputLine)
            {
                var m = _rx.Match(fullInputLine);
                this.minTimes = int.Parse(m.Groups[1].Value);
                this.maxTimes = int.Parse(m.Groups[2].Value);
                this.charRequired = m.Groups[3].Value[0];
                this.password = m.Groups[4].Value;
            }

            public bool IsValid()
            {
                int timesCharInPassword = password.Count(c => c == this.charRequired);
                return minTimes <= timesCharInPassword && timesCharInPassword <= maxTimes;
            }

            public bool IsValidForTheRealThingThatIsDownTheStreet()
            {
                return (password[minTimes - 1] == charRequired) ^ (password[maxTimes - 1] == charRequired);
            }
        }

        public static void ExecuteDay2(string fileLocation = "PuzzleInput/Day2.txt")
        {
            

            string[] lines = File.ReadAllLines(fileLocation);

            List<TobogganPassword> passwords = new List<TobogganPassword>();

            foreach (var line in lines)
            {
                passwords.Add(new TobogganPassword(line));
            }

            Logger.LogMessage(LogLevel.ANSWER, "2A: Valid Passwords: " + passwords.Count(p => p.IsValid()));
            Logger.LogMessage(LogLevel.ANSWER, "2B: Valid Passwords: " + passwords.Count(p => p.IsValidForTheRealThingThatIsDownTheStreet()));

        }
    }
}
