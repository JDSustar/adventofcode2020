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
    public class Day9
    {
        public class XMAS
        {
            public int currentIndex = 0;
            public List<long> dataStream = new List<long>();
            public List<long> validChecksums = new List<long>();
            public long CurrentValue => dataStream[currentIndex];
            public bool CurrentValueValid => IsCurrentIndexValid();

            public XMAS(string fullText)
            {
                dataStream = fullText.Split(Environment.NewLine).Select(long.Parse).ToList();

                currentIndex = 25;
            }

            private void RecalculateValidChecksums(int lookback = 25)
            {
                validChecksums.Clear();

                for (int i = currentIndex - lookback; i < currentIndex; i++)
                {
                    for (int j = currentIndex - lookback; j < currentIndex; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        else
                        {
                            validChecksums.Add(dataStream[i] + dataStream[j]);
                        }
                    }
                }
            }

            private bool IsCurrentIndexValid()
            {
                RecalculateValidChecksums();
                return validChecksums.Contains(dataStream[currentIndex]);
            }

            public long FindValueSumInContiguousData(long searchValue)
            {
                for (int i = 0; i < dataStream.Count; i++)
                {
                    int length = 1;

                    while (dataStream.Skip(i).Take(length).Sum() < searchValue)
                    {
                        length++;
                    }

                    if (dataStream.Skip(i).Take(length).Sum() == searchValue)
                    {
                        return dataStream.Skip(i).Take(length).Max() + dataStream.Skip(i).Take(length).Min();
                    }
                }

                return -1;
            }
        }

        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day9.txt")
        {
            string allText = File.ReadAllText(fileLocation);

            var x = new XMAS(allText);

            while (x.CurrentValueValid)
            {
                x.currentIndex++;
            }

            Logger.LogMessage(LogLevel.ANSWER, "9A: " + x.CurrentValue);
            
            

            Logger.LogMessage(LogLevel.ANSWER, "9B: " + x.FindValueSumInContiguousData(x.CurrentValue));
        }
    }
}
