using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode2020
{
    public class Day3
    {
        public class Forest
        {
            
            private List<List<char>> _forestTile = new List<List<char>>();

            public void AddRow(string l)
            {
                _forestTile.Add(new List<char>(l));
            }

            public int GetTreesWhenTraverseForestAtSlope(int right, int down)
            { 
                var currentPosition = new Point(0, 0);
                var treesEncountered = 0;

                while (currentPosition.Y < _forestTile.Count)
                {
                    if (_forestTile[currentPosition.Y][currentPosition.X] == '#')
                    {
                        treesEncountered++;
                    }

                    currentPosition.X = (currentPosition.X + right) % _forestTile[0].Count;
                    currentPosition.Y += down;
                }

                return treesEncountered;
            }
        }


    public static void ExecuteDay(string fileLocation = "PuzzleInput/Day3.txt")
        {
            string[] lines = File.ReadAllLines(fileLocation);

            var f = new Forest();

            foreach (var line in lines)
            {
                f.AddRow(line);
            }

            uint treesForPart2 = (uint)f.GetTreesWhenTraverseForestAtSlope(1, 1) * (uint)f.GetTreesWhenTraverseForestAtSlope(3, 1) *
                                 (uint)f.GetTreesWhenTraverseForestAtSlope(5, 1) * (uint)f.GetTreesWhenTraverseForestAtSlope(7, 1) *
                                 (uint)f.GetTreesWhenTraverseForestAtSlope(1, 2);

            Logger.LogMessage(LogLevel.ANSWER, "3A: Valid Passwords: " + f.GetTreesWhenTraverseForestAtSlope(3,1));
            Logger.LogMessage(LogLevel.ANSWER, "2B: Valid Passwords: " + treesForPart2);
        }
    }
}
