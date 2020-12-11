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
    public class Day11
    {
        public class SeatingArea
        {
            public List<List<Seat>> Seats = new List<List<Seat>>();

            public int NumSeatsOccupied => Seats.Sum(sl => sl.Count(s => s.State == Seat.SeatState.Occupied));

            public SeatingArea(string fulltext)
            {
                foreach (var s in fulltext.Split(Environment.NewLine))
                {
                    Seats.Add(new List<Seat>());
                    foreach (var c in s)
                    {
                        Seats.Last().Add(new Seat(c));
                    }
                }
            }

            public bool Update(int neighborsRequired = 4, bool useVisibleNeighborsOnly = false)
            {
                bool somethingChanged = false;
                for (int y = 0; y < Seats.Count; y++)
                {
                    for (int x = 0; x < Seats[y].Count; x++)
                    {
                        if (Seats[y][x].State == Seat.SeatState.Empty)
                        {
                            int neighbors = useVisibleNeighborsOnly
                                ? GetNumOccupiedVisibleNeighbors(x, y)
                                : GetNumOccupiedNeighbors(x, y);

                            if (neighbors == 0) {
                                Seats[y][x].NextState = Seat.SeatState.Occupied;
                                somethingChanged = true;
                            }
                        }
                        else if (Seats[y][x].State == Seat.SeatState.Occupied )
                        {
                            int neighbors = useVisibleNeighborsOnly
                                    ? GetNumOccupiedVisibleNeighbors(x, y)
                                    : GetNumOccupiedNeighbors(x, y);

                            if (neighbors >= neighborsRequired)
                            {
                                Seats[y][x].NextState = Seat.SeatState.Empty;
                                somethingChanged = true;
                            }
                        }
                    }
                }

                foreach (var sl in Seats)
                {
                    foreach (var seat in sl)
                    {
                        seat.CommitState();
                    }
                }

                return somethingChanged;
            }

            private int GetNumOccupiedNeighbors(int x, int y)
            {
                int numOccupied = 0;

                for (int checky = y - 1; checky <= y + 1; checky++)
                {
                    for (int checkx = x - 1; checkx <= x + 1; checkx++)
                    {
                        if (checkx == x && checky == y)
                        {
                            continue;
                        }

                        if (checky < 0 || checky >= Seats.Count || checkx < 0 || checkx >= Seats[checky].Count)
                        {
                            continue;
                        }

                        if (Seats[checky][checkx].State == Seat.SeatState.Occupied)
                        {
                            numOccupied++;
                        }
                    }
                }

                return numOccupied;
            }

            private int GetNumOccupiedVisibleNeighbors(int x, int y)
            {
                int numOccupied = 0;

                for (int slopey = -1; slopey <= 1; slopey++)
                {
                    for (int slopex = -1; slopex <= 1; slopex++)
                    {
                        if (slopex == 0 && slopey == 0)
                        {
                            continue;
                        }

                        int checkx = x;
                        int checky = y;

                        do
                        {
                            checkx += slopex;
                            checky += slopey;
                        } while (checky >= 0 && checky < Seats.Count && checkx >= 0 && checkx < Seats[checky].Count && Seats[checky][checkx].State == Seat.SeatState.Floor);

                        if (checky < 0 || checky >= Seats.Count || checkx < 0 || checkx >= Seats[checky].Count || Seats[checky][checkx].State == Seat.SeatState.Floor)
                        {
                            continue;
                        }

                        if (Seats[checky][checkx].State == Seat.SeatState.Occupied)
                        {
                            numOccupied++;
                        }
                    }
                }

                return numOccupied;
            }

            public override string ToString()
            {
                string s = "";

                for (int y = 0; y < Seats.Count; y++)
                {
                    for (int x = 0; x < Seats[y].Count; x++)
                    {
                        switch (Seats[y][x].State)
                        {
                            case Seat.SeatState.Empty:
                                s += "L";
                                break;
                            case Seat.SeatState.Floor:
                                s += ".";
                                break;
                            case Seat.SeatState.Occupied:
                                s += "#";
                                break;
                        }
                    }

                    s += "\n";
                }

                return s;
            }
        }

        public class Seat
        {
            public enum SeatState
            {
                Floor, Empty, Occupied
            }

            public SeatState State;
            public SeatState NextState;

            public Seat(char state)
            {
                switch (state)
                {
                    case '.':
                        this.State = SeatState.Floor;
                        break;
                    case 'L':
                        this.State = SeatState.Empty;
                        break;
                }
            }

            public void CommitState()
            {
                State = NextState;
            }
        }

        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day11.txt")
        {
            string alltext = File.ReadAllText(fileLocation);

            SeatingArea sa = new SeatingArea(alltext);

            while (sa.Update()) ;

            Logger.LogMessage(LogLevel.ANSWER, "10A: " + sa.NumSeatsOccupied);

            var sa2 = new SeatingArea(alltext);

            while (sa2.Update(5, true)) ;

            Logger.LogMessage(LogLevel.ANSWER, "10B: " + sa2.NumSeatsOccupied);
        }
    }
}
