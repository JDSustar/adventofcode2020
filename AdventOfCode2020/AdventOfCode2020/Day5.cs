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
    public class Day5
    {
        public class BoardingPass : IComparable
        {
            private string FrontBackString => _fullSeatString.Substring(0, 7);

            private string LeftRightString => _fullSeatString.Substring(7);

            private string _fullSeatString;

            public BoardingPass(string seatString)
            {
                this._fullSeatString = seatString;
            }

            public int GetFrontBackPosition()
            {
                int min = 1;
                int max = 128;

                for (int i = 0; i < FrontBackString.Length - 1; i++)
                {
                    if (FrontBackString[i] == 'F')
                    {
                        max = max - ((max - min + 1) / 2);
                    }
                    else if (FrontBackString[i] == 'B')
                    {
                        min = min + ((max - min + 1) / 2);
                    }
                }

                if (FrontBackString[^1] == 'B')
                {
                    return max - 1;
                }
                else if (FrontBackString[^1] == 'F')
                {
                    return min - 1;
                }
                else
                {
                    return -1;
                }
            }


            public int GetLeftRightPosition()
            {
                int min = 1;
                int max = 8;

                for (int i = 0; i < LeftRightString.Length - 1; i++)
                {
                    if (LeftRightString[i] == 'L')
                    {
                        max = max - ((max - min + 1) / 2);
                    }
                    else if (LeftRightString[i] == 'R')
                    {
                        min = min + ((max - min + 1) / 2);
                    }
                }

                if (LeftRightString[^1] == 'R')
                {
                    return max - 1;
                }
                else if (LeftRightString[^1] == 'L')
                {
                    return min - 1;
                }
                else
                {
                    return -1;
                }
            }

            public int CompareTo(object obj)
            {
                if (obj is BoardingPass)
                {
                    var bp2 = (BoardingPass) obj;

                    if (this.SeatID > bp2.SeatID)
                    {
                        return 1;
                    }
                    else if (this.SeatID < bp2.SeatID)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                return -1;
            }

            public int SeatID => this.GetFrontBackPosition() * 8 + this.GetLeftRightPosition();
        
        }

        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day5.txt")
        {
            string[] lines = File.ReadAllLines(fileLocation);

            List<BoardingPass> bps = new List<BoardingPass>();

            foreach (var line in lines)
            {
                bps.Add(new BoardingPass(line));
            }

            var max = 0;

            foreach (var bp in bps)
            {
                if (bp.SeatID > max) max = bp.SeatID;
            }

            Logger.LogMessage(LogLevel.ANSWER, "5A: " + max);
            
            bps.Sort();

            int current = bps.First().SeatID;

            foreach (var bp in bps)
            {

                if (bp.SeatID != current && bp.SeatID != current + 1)
                {
                    break;
                }
                else
                {
                    current = bp.SeatID;
                }
            }

            Logger.LogMessage(LogLevel.ANSWER, "5B: " + (current + 1));
        }
    }
}
