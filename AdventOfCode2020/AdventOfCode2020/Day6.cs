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
    public class Day6
    {
        public class AnswerGroup
        {
            public Dictionary<char, int> questions = new Dictionary<char, int>();
            public int NumOfAnswers = 0;
            public AnswerGroup(string groupString)
            {
                string[] answers = groupString.Split(Environment.NewLine);

                foreach (var a in answers)
                {
                    NumOfAnswers++;
                    foreach (var c in a)
                    {
                        if (questions.ContainsKey(c))
                        {
                            questions[c]++;
                        }
                        else
                        {
                            questions.Add(c, 1);
                        }
                    }
                }
            }

            public int NumQuestionsInGroup => questions.Keys.Count;

            public int NumQuestionsAllYes => questions.Count(kv => kv.Value == this.NumOfAnswers);
        }

        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day6.txt")
        {
            string[] groupTexts = File.ReadAllText(fileLocation).Split(Environment.NewLine + Environment.NewLine);

            var ags = new List<AnswerGroup>();

            foreach (var gt in groupTexts)
            {
                ags.Add(new AnswerGroup(gt));
            }

            Logger.LogMessage(LogLevel.ANSWER, "6A: " + ags.Sum(ag => ag.NumQuestionsInGroup));
            Logger.LogMessage(LogLevel.ANSWER, "6B: " + ags.Sum(ag => ag.NumQuestionsAllYes));
        }
    }
}
