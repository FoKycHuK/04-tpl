using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
    public class Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(Point))
                return false;
            var point = (Point)obj;
            return point.X == X && point.Y == Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
