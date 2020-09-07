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
            for (int i = 0; i < 1000; i++)
            {
                IterateSimulation();
            }
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

        private void IterateSimulation()
        {
            int Neighbours = 0;
            Cell Dummy;
            bool Exists;
            for (int x = StarterSize * -1; x < StarterSize; x++)
            {
                for (int y = StarterSize * -1; y < StarterSize; y++)
                {
                    Neighbours = GetAliveNeighbours(x, y);
                    switch (Neighbours)
                    {
                        case 2:
                            break;
                        case 3:
                            Exists = cells.TryGetValue(new CellCoords(x, y), out Dummy);
                            if (Exists == false)
                            {
                                //TODO: add alive Cell if dead and heat+
                            }
                            break;
                        default:
                            Exists = cells.TryGetValue(new CellCoords(x, y), out Dummy);
                            if (Exists)
                            {
                                //TODO: kill cell if alive and heat-
                            }
                            break;
                    }
                }
            }
        }

        private int GetAliveNeighbours(int CoordX, int CoordY)
        {
            int Neighbours = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        Cell dummy;
                        bool alive;
                        alive = cells.TryGetValue(new CellCoords(CoordX + x, CoordY + y), out dummy);
                        if (alive)
                        {
                            Neighbours++;
                        }
                    }
                }
            }
            return Neighbours;
        }
    }
}