using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace AdventOfCode2020
{
    public class Day1
    {
        private static int FindTwoEntrySum2020(List<int> expenseReportEntries)
        {
            expenseReportEntries.Sort();
            int topIndex = expenseReportEntries.Count - 1;
            int bottomIndex = 0;


            while (expenseReportEntries[bottomIndex] + expenseReportEntries[topIndex] != 2020)
            {
                if (expenseReportEntries[bottomIndex] + expenseReportEntries[topIndex] > 2020)
                {
                    topIndex--;
                }
                else if (expenseReportEntries[bottomIndex] + expenseReportEntries[topIndex] < 2020)
                {
                    bottomIndex++;
                }
                else if (topIndex < bottomIndex)
                {
                    throw new Exception("1A: Answer not possible");
                }
            }

            return expenseReportEntries[bottomIndex] * expenseReportEntries[topIndex];
        }

        private static int FindThreeEntrySum2020(List<int> expenseReportEntries)
        {
            for (int i = 0; i < expenseReportEntries.Count; i++)
            {
                for (int j = 0; j < expenseReportEntries.Count; j++)
                {
                    for (int k = 0; k < expenseReportEntries.Count; k++)
                    {
                        if (expenseReportEntries[i] + expenseReportEntries[j] + expenseReportEntries[k] == 2020)
                            return expenseReportEntries[i] * expenseReportEntries[j] * expenseReportEntries[k];
                    }
                }
            }

            throw new Exception("1B: Answer not possible");
        }

        public static void ExecuteDay1(string fileLocation = "PuzzleInput/Day1.txt")
        {
            string[] lines = File.ReadAllLines(fileLocation);

            List<int> expenseReportLines = new List<int>();

            foreach (var line in lines)
            {
                expenseReportLines.Add(int.Parse(line));
            }

            Logger.LogMessage(LogLevel.ANSWER, "1A: Expense Report Checksum: " + FindTwoEntrySum2020(expenseReportLines));
            Logger.LogMessage(LogLevel.ANSWER, "1B: Expense Report Checksum: " + FindThreeEntrySum2020(expenseReportLines));
        }
    }
}
