using System;
using System.Collections.Generic;

namespace SlimeStrategy.BattleMap.Grid
{
    public class GridCoordinate
    {
        public int X { get; }
        public int Z { get; }

        public GridCoordinate(int x, int z)
        {
            this.X = x;
            this.Z = z;
        }

        public override string ToString()
        {
            return $"{{(X={X}, Z={Z})}}";
        }
    }

    public class GridCoordinateEqualityComparer : IEqualityComparer<GridCoordinate>
    {
        public bool Equals(GridCoordinate x, GridCoordinate y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.X == y.X && x.Z == y.Z;
        }

        public int GetHashCode(GridCoordinate obj)
        {
            return HashCode.Combine(obj.X, obj.Z);
        }
    }
}