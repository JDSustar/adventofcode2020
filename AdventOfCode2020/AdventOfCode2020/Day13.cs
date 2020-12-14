using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class Day13
    {
        public class BusSchedule
        {
            List<Bus> Busses = new List<Bus>();

            public BusSchedule(string busSchedule)
            {
                string[] busStrings = busSchedule.Split(',');
                int position = 0;

                foreach (var busString in busStrings)
                {
                    if (busString != "x")
                        Busses.Add(new Bus(int.Parse(busString), position));

                    position++;
                }
            }

            public Bus GetFirstBusAfterTime(int time)
            {
                int min = int.MaxValue;
                Bus minBus = null;

                foreach (var bus in Busses)
                {
                    if (bus.GetFirstVisitAfterTime(time) < min)
                    {
                        min = bus.GetFirstVisitAfterTime(time);
                        minBus = bus;
                    }    
                }

                return minBus;
            }

            public long FindSequentialTime()
            {
                long[] n = Busses.Select(b => (long) b.ID).ToArray();
                long[] a = Busses.Select(b => (long)b.ID - (long) b.Position).ToArray();

                long answer = ChineseRemainderTheorem.Solve(n, a);

                return answer;
            }
        }

        public class Bus
        {
            public int ID;
            public int Position;

            public Bus(int id, int position)
            {
                this.ID = id;
                this.Position = position;
            }

            public int GetFirstVisitAfterTime(int time)
            {
                int currentVisit = 0;

                while (currentVisit < time)
                {
                    currentVisit += this.ID;
                }

                return currentVisit;
            }

            public bool DoesBusArriveAt(long time)
            {
                return (time % this.ID) == 0;
            }
        }
        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day13.txt")
        {
            string[] inputs = File.ReadAllText(fileLocation).Split(Environment.NewLine);

            var time = int.Parse(inputs[0]);
            var busSchedule = inputs[1];

            var bs = new BusSchedule(busSchedule);
            var firstBusTime = bs.GetFirstBusAfterTime(time);

            Logger.LogMessage(LogLevel.ANSWER, "13A: " + ((firstBusTime.ID) * (firstBusTime.GetFirstVisitAfterTime(time) - time)));

            Logger.LogMessage(LogLevel.ANSWER, "13B: " + bs.FindSequentialTime());
        }
    }
}