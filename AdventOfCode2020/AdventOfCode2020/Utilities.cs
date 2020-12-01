using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public static class Utilities
    {
        public static int GetManhattenDistance(int a, int b)
        {
            return Math.Abs(a) + Math.Abs(b);
        }

        // https://stackoverflow.com/a/10629938
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1).SelectMany(t => list.Where(o => !t.Contains(o)), (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            if (a == 0)
                return b;
            else
                return a;
        }

        public static double GetAngleInRadians(int x, int y)
        {
            // Get angle as defined on https://docs.microsoft.com/en-us/dotnet/api/system.math.atan2?view=netframework-4.8
            double angle = Math.Atan2(y, x);

            // Shift the coordinate system counter-clockwise 90 degrees
            angle = angle - (Math.PI / 2);

            // Adjust everything to be positive
            if (angle < 0)
            {
                angle = (Math.PI * 2) + angle;
            }

            // Reverse it so the coordinate system is clockwise
            angle = (2 * Math.PI) - angle;

            // Ensure nothing is >= 2pi
            if (angle >= (Math.PI) * 2)
            {
                angle = angle - (Math.PI * 2);
            }

            return angle;
        }

        // https://stackoverflow.com/a/9894743
        public static T[,] GetNew2DArray<T>(int x, int y, T initialValue)
        {
            T[,] nums = new T[x, y];
            for (int i = 0; i < x * y; i++) nums[i % x, i / x] = initialValue;
            return nums;
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj is Point point)
            {
                return point.X == this.X && point.Y == this.Y;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }
    }
}
