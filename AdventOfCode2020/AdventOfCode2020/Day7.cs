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
    public class Day7
    {
        public class Bag
        {
            Regex bagRegex = new Regex(@"(.*)\sbags contain\s(.*\sbags?)+.");
            Regex bagsContainedRegex = new Regex(@"(\d)\s(.*?)\sbags?");

            public Dictionary<string, int> BagsContained = new Dictionary<string, int>();

            public string BagName;
            private string _originalBagDefinition;

            public Bag(string bagDefinitionString)
            {
                this._originalBagDefinition = bagDefinitionString;

                var m = bagRegex.Match(bagDefinitionString);
                BagName = m.Groups[1].Value;

                var m2 = bagsContainedRegex.Matches(m.Groups[2].Value);
                if (m2.Count > 0)
                {
                    foreach (Match bagmatch in m2)
                    {
                        BagsContained.Add(bagmatch.Groups[2].Value, int.Parse(bagmatch.Groups[1].Value));
                    }
                }
            }

            public override string ToString()
            {
                return _originalBagDefinition;
            }
        }

        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day7.txt")
        {
            string[] bagLines = File.ReadAllText(fileLocation).Split(Environment.NewLine);

            List<Bag> bags = new List<Bag>();

            foreach (var bagLine in bagLines)
            {
                bags.Add(new Bag(bagLine));
            }

            int bagsContainingShinyGold = 0;

            foreach (var bag in bags)
            {
                
                if (DoesBagContain(bags, bag))
                {
                    Logger.LogMessage(LogLevel.DEBUG, bag.BagName + " contains shiny gold");
                    bagsContainingShinyGold++;
                }
                else
                {
                    Logger.LogMessage(LogLevel.DEBUG, bag.BagName + " does not contains shiny gold");
                }
            }
            
            Logger.LogMessage(LogLevel.ANSWER, "7A: " + bagsContainingShinyGold);
            Logger.LogMessage(LogLevel.ANSWER, "7B: " + NumBagsContained(bags, bags.FirstOrDefault(b => b.BagName == "shiny gold")));
        }

        public static int NumBagsContained(List<Bag> bags, Bag searchBag)
        {
            if (searchBag.BagsContained.Count == 0)
            {
                return 0;
            }
            else
            {
                int result = searchBag.BagsContained.Sum(kv => kv.Value);
                foreach (var kv in searchBag.BagsContained)
                {
                    Bag nextBag = bags.FirstOrDefault(b => b.BagName == kv.Key);
                    result += NumBagsContained(bags, nextBag) * kv.Value;
                }

                return result;
            }
        }

        public static bool DoesBagContain(List<Bag> bags, Bag searchBag, string targetBag = "shiny gold")
        {
            if (searchBag.BagsContained.Count == 0)
            {
                return false;
            }
            else if (searchBag.BagsContained.Keys.Contains(targetBag))
            {
                return true;
            }
            else
            {
                bool result = false;
                foreach (var bagsContainedKey in searchBag.BagsContained.Keys)
                {
                    Bag nextBag = bags.FirstOrDefault(b => b.BagName == bagsContainedKey);
                    result = DoesBagContain(bags, nextBag, targetBag) || result;
                }

                return result;
            }
        }
    }
}
