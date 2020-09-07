
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace conway.lib {
    class CellCoords
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public CellCoords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class CellCoordsComparer : IEqualityComparer<CellCoords>
    {
        public bool Equals([AllowNull] CellCoords first, [AllowNull] CellCoords second)
        {
            if (first == null && second == null)
                return true;
            else if (first == null || second == null)
                return false;
            else if (first.x == second.x && first.y == second.y)
                return true;
            else
                return false;
        }

        public int GetHashCode([DisallowNull] CellCoords obj)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ obj.x.GetHashCode();
                hash = (hash * 16777619) ^ obj.y.GetHashCode();
                return hash;
            }
        }
    }

    class Cell
    {
        public Cell()
        {
        }
    }
}