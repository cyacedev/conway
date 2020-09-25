
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Conway
{
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

    class CellAverageNeighbours{
        public List<int> _neighbours { get; private set;}
        public float averageNeighbourNumber { get; private set; }

        public CellAverageNeighbours(){
            this._neighbours = new List<int>();
        }

        public void AddCellNeighbourEntry(int neighbours){
            this._neighbours.Add(neighbours);
        }

        public void CalculateAverage(){
            int allNeighbours = 0;
            foreach(int numOfNeighbour in _neighbours){
                allNeighbours += numOfNeighbour;
            }
            this.averageNeighbourNumber = allNeighbours / (float)_neighbours.Count;
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

    class Cell
    {
        public Cell()
        {
        }
    }
}