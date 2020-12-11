using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode2020
{
    public class Day10
    {
        struct JoltageAdapter
        {
            public readonly int Rating;
            public int MinimumInput => Math.Max(0, Rating - 3);

            public JoltageAdapter(int rating)
            {
                this.Rating = rating;
            }
        }

        public class AdapterChain
        {
            private int min;
            private int max;

            public AdapterChain(int min, int max)
            {
                this.min = min;
                this.max = max;
            }

            public int PossibleCombinations()
            {
                if (max - min == 1)
                {
                    return 1;
                }
                else if (max - min == 2)
                {
                    return 2;
                }
                else if (max - min == 3)
                {
                    return 4;
                }
                else if (max - min == 4)
                {
                    return 7;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            public override string ToString()
            {
                return "FROM " + this.min + " TO " + this.max + " : " + this.PossibleCombinations() + " Combinations";
            }
        }
    

        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day10.txt")
        {
            List<int> joltages = File.ReadAllText(fileLocation).Split(Environment.NewLine).Select(int.Parse).ToList();
            joltages.Add(0);
            joltages.Add(joltages.Max() + 3);
            joltages.Sort();

            int gap1 = 0;
            int gap3 = 0;

            for (int i = 1; i < joltages.Count; i++)
            {
                if (joltages[i] - joltages[i - 1] == 1)
                {
                    gap1++;
                }
                else if (joltages[i] - joltages[i - 1] == 3)
                {
                    gap3++;
                }
                else
                {
                    throw new Exception("GAP BIGGER THAN 3");
                }
            }

            Logger.LogMessage(LogLevel.ANSWER, "10A: " + gap1 * gap3);

            List<AdapterChain> adapterChains = new List<AdapterChain>();

            int currentMin = 0;
            int currentMax = 1;

            for (int i = 1; i < joltages.Count; i++)
            {
                if (joltages[i] - joltages[i - 1] == 1)
                {
                    currentMax = joltages[i];
                }
                else if (joltages[i] - joltages[i - 1] == 3)
                {
                    if (currentMin < currentMax)
                        adapterChains.Add(new AdapterChain(currentMin, currentMax));
                    currentMin = joltages[i];
                }
                else
                {
                    throw new Exception("GAP BIGGER THAN 3");
                }
            }

            Logger.LogMessage(LogLevel.ANSWER, "10B: " + adapterChains.Aggregate((long)1, (acc, ac) => (long)acc * (long)ac.PossibleCombinations()));
        }
    }
}
