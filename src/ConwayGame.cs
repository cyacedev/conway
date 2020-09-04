using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace conway.lib
{
    public class ConwayGame
    {
        private readonly int StarterSize;
        private Dictionary<CellCoords, Cell> cells;

        public ConwayGame(int StarterSize)
        {
            this.StarterSize = StarterSize;
            cells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
        }

        public void Run()
        {
            GenerateDefinedAmountOfCells();
        }

        private void GenerateDefinedAmountOfCells()
        {
            for (int i = 0; i < StarterSize / 2; i++)
            {
                bool added = false;
                do
                {
                    var Random = new System.Random();
                    var x = Random.Next(StarterSize * -1, StarterSize);
                    var y = Random.Next(StarterSize * -1, StarterSize);
                    added = cells.TryAdd(new CellCoords(x, y), new Cell());
                } while (added == false);
            }
        }
    }

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