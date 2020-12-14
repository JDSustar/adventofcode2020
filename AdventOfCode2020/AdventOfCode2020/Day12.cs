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
    public enum CardinalDirections
    {
        North = 0, South = 2, East = 1, West = 3
    }

    public enum TurningDirections
    {
        Left = -1, Right =  1
    }

    public class Ferry
    {
        public Point StartingPosition = new Point(0, 0);
        public Point CurrentWaypoint = new Point(10, 1);
        public CardinalDirections CurrentHeading = CardinalDirections.East;
        public List<string> RouteInstructions;
        public Point CurrentPosition = new Point(0,0);

        public Ferry(string routeAllText)
        {
            RouteInstructions = routeAllText.Split(Environment.NewLine).ToList();
        }

        public void Travel()
        {
            foreach (var routeInstruction in RouteInstructions)
            {
                var action = routeInstruction[0];
                int value = int.Parse(routeInstruction.Substring(1));

                switch (action)
                {
                    case 'L':
                        Turn(TurningDirections.Left, value/90);
                        break;
                    case 'R':
                        Turn(TurningDirections.Right, value / 90);
                        break;
                    case 'F':
                        Move(value);
                        break;
                    case 'N':
                        Move(value, CardinalDirections.North);
                        break;
                    case 'S':
                        Move(value, CardinalDirections.South);
                        break;
                    case 'W':
                        Move(value, CardinalDirections.West);
                        break;
                    case 'E':
                        Move(value, CardinalDirections.East);
                        break;
                }
            }
        }

        public void TravelUsingWaypoint()
        {
            foreach (var routeInstruction in RouteInstructions)
            {
                Logger.LogMessage(LogLevel.DEBUG, CurrentPosition + " : " + CurrentWaypoint);
                var action = routeInstruction[0];
                int value = int.Parse(routeInstruction.Substring(1));

                switch (action)
                {
                    case 'L':
                        RotateWaypoint(TurningDirections.Left, value);
                        break;
                    case 'R':
                        RotateWaypoint(TurningDirections.Right, value);
                        break;
                    case 'F':
                        MoveUsingWaypoint(value);
                        break;
                    case 'N':
                        MoveWaypoint(value, CardinalDirections.North);
                        break;
                    case 'S':
                        MoveWaypoint(value, CardinalDirections.South);
                        break;
                    case 'W':
                        MoveWaypoint(value, CardinalDirections.West);
                        break;
                    case 'E':
                        MoveWaypoint(value, CardinalDirections.East);
                        break;
                }
            }
        }

        private void Move(int units)
        {
            Move(units, CurrentHeading);
        }

        private void Move(int units, CardinalDirections cd)
        {
            switch (cd)
            {
                case CardinalDirections.North:
                    CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y + units);
                    break;
                case CardinalDirections.East:
                    CurrentPosition = new Point(CurrentPosition.X + units, CurrentPosition.Y);
                    break;
                case CardinalDirections.South:
                    CurrentPosition = new Point(CurrentPosition.X, CurrentPosition.Y - units);
                    break;
                case CardinalDirections.West:
                    CurrentPosition = new Point(CurrentPosition.X - units, CurrentPosition.Y);
                    break;
            }
        }

        private void Turn(TurningDirections td, int times)
        {
            var currentHeadingInt = ((((int) td * times) + (int) CurrentHeading + 4) % 4);

            CurrentHeading = (CardinalDirections)currentHeadingInt;
        }

        private void MoveUsingWaypoint(int units)
        {
            for (int i = 0; i < units; i++)
            {
                CurrentPosition = new Point(CurrentWaypoint.X + CurrentPosition.X, CurrentWaypoint.Y + CurrentPosition.Y);
            }
        }

        private void MoveWaypoint(int units, CardinalDirections cd)
        {
            switch (cd)
            {
                case CardinalDirections.North:
                    CurrentWaypoint = new Point(CurrentWaypoint.X, CurrentWaypoint.Y + units);
                    break;
                case CardinalDirections.East:
                    CurrentWaypoint = new Point(CurrentWaypoint.X + units, CurrentWaypoint.Y);
                    break;
                case CardinalDirections.South:
                    CurrentWaypoint = new Point(CurrentWaypoint.X, CurrentWaypoint.Y - units);
                    break;
                case CardinalDirections.West:
                    CurrentWaypoint = new Point(CurrentWaypoint.X - units, CurrentWaypoint.Y);
                    break;
            }
        }

        private void RotateWaypoint(TurningDirections td, int degrees)
        {
            var rotateDegrees = td == TurningDirections.Right ? degrees : -degrees;
            var rotateRadians = rotateDegrees * Math.PI / 180;
            
            var newX = (int)Math.Round(CurrentWaypoint.X * Math.Cos(rotateRadians) + CurrentWaypoint.Y * Math.Sin(rotateRadians));
            var newY = (int)Math.Round(-CurrentWaypoint.X * Math.Sin(rotateRadians) + CurrentWaypoint.Y * Math.Cos(rotateRadians));

            CurrentWaypoint = new Point(newX, newY);
        }
    }

    public class Day12
    { 
        

        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day12.txt")
        {
            string alltext = File.ReadAllText(fileLocation);

            var f = new Ferry(alltext);
            f.Travel();

            Logger.LogMessage(LogLevel.ANSWER, "12A: " + (Utilities.GetManhattenDistance(0, f.CurrentPosition.X) + Utilities.GetManhattenDistance(0, f.CurrentPosition.Y)));

            var f2 = new Ferry(alltext);
            f2.TravelUsingWaypoint();

            Logger.LogMessage(LogLevel.ANSWER, "12B: " + (Utilities.GetManhattenDistance(0, f2.CurrentPosition.X) + Utilities.GetManhattenDistance(0, f2.CurrentPosition.Y)));
        }
    }
}
