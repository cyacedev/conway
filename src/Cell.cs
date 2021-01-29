
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Conway
{
    public class CellCoords
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public CellCoords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    public class ChangedCellCoords
    {
        public CellCoords cellCoords { get; private set; }
        public bool newLife { get; private set; }

        public ChangedCellCoords(CellCoords changedCoords, bool newLife)
        {
            this.cellCoords = changedCoords;
            this.newLife = newLife;
        }
    }

    class CellAverageNeighbours
    {
        public List<int> _neighbours { get; private set; }
        public float averageNeighbourNumber { get; private set; }

        public CellAverageNeighbours()
        {
            this._neighbours = new List<int>();
        }

        public void AddCellNeighbourEntry(int neighbours)
        {
            this._neighbours.Add(neighbours);
        }

        public void CalculateAverage()
        {
            int allNeighbours = 0;
            foreach (int numOfNeighbour in _neighbours)
            {
                allNeighbours += numOfNeighbour;
            }
            this.averageNeighbourNumber = allNeighbours / (float)_neighbours.Count;
        }

    }

    class CellDensity
    {
        public CellCoords fromX { get; set; }
        public CellCoords fromY { get; set; }
        public CellCoords toX { get; set; }
        public CellCoords toY { get; set; }
        public int cellCount { get; set; }
        public float cellDensity { get; set; }
        public CellDensity(int cellCount)
        {
            fromX = new CellCoords(int.MaxValue, 0);
            fromY = new CellCoords(0, int.MaxValue);
            toX = new CellCoords(int.MinValue, 0);
            toY = new CellCoords(int.MinValue, 0);
            this.cellCount = cellCount;
        }

        public void AddCellCoordToCheck(CellCoords checkCoords)
        {
            if (checkCoords.x < fromX.x)
            {
                fromX = checkCoords;
            }

            if (checkCoords.y < fromY.y)
            {
                fromY = checkCoords;
            }
            if (checkCoords.x > toX.x)
            {
                toX = checkCoords;
            }
            if (checkCoords.y > toY.y)
            {
                toY = checkCoords;
            }
        }

        public void CalculateDensity()
        {
            int length = toX.x - fromX.x + 1;
            int height = toY.y - fromY.y + 1;
            int numberOfFields = length * height;
            if (numberOfFields == 0)
            {
                Console.WriteLine($"length * height = 0: fromx({fromX.x}) tox({toX.x}), fromy({fromY.y}) toy({toY.y})");
            }
            this.cellDensity = this.cellCount / (float)numberOfFields;
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
                //new hash introduced, see https://stackoverflow.com/a/13871379
                var x = obj.x;
                var y = obj.y;
                Int32 xHash = (Int32)(x >= 0 ? 2 * x : -2 * x - 1);
                Int32 yHash = (Int32)(y >= 0 ? 2 * y : -2 * y - 1);
                Int32 hash = ((xHash >= yHash ? xHash * xHash + xHash + yHash : xHash + yHash * yHash) / 2);
                return x < 0 && y < 0 || x >= 0 && y >= 0 ? hash : -hash - 1;
            }
        }
    }

    public class Cell
    {
        public Cell()
        {
        }
    }

}