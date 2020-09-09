using System;
using System.Collections.Generic;

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
            GenerateCellsWithProbability(50);
            Console.WriteLine($"Generated Cells: {cells.Count}");
            for (int i = 0; i < 1000; i++)
            {
                IterateSimulation();
            }
            Console.WriteLine($"Alive after 1000 iterations: {cells.Count}");
        }

        private void GenerateDefinedAmountOfCells(int AmountOfCells)
        {
            for (int i = 0; i < AmountOfCells; i++)
            {
                bool added = false;
                do
                {
                    var Random = new System.Random();
                    var x = Random.Next(StarterSize * -1, StarterSize + 1);
                    var y = Random.Next(StarterSize * -1, StarterSize + 1);
                    added = cells.TryAdd(new CellCoords(x, y), new Cell());
                } while (added == false);
            }
        }

        private void GenerateCellsWithProbability(int PromilleMax)
        {
            for (int x = StarterSize * -1; x <= StarterSize; x++)
            {
                for (int y = StarterSize * -1; y <= StarterSize; y++)
                {
                    var Random = new System.Random();
                    if (Random.Next(1001) <= PromilleMax)
                    {
                        cells.Add(new CellCoords(x, y), new Cell());
                    }
                }
            }
        }

        private void IterateSimulation()
        {
            var newCells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
            foreach (System.Collections.Generic.KeyValuePair<conway.lib.CellCoords, conway.lib.Cell> cell in cells)
            {
                for (int y = cell.Key.y - 1; y <= (cell.Key.y + 1); y++)
                {
                    for (int x = cell.Key.x - 1; x <= (cell.Key.x + 1); x++)
                    {
                        int Neighbours = GetAliveNeighbours(x, y);
                        Cell CurrentCell;
                        bool Exists = cells.TryGetValue(new CellCoords(x, y), out CurrentCell);
                        switch (Neighbours)
                        {
                            case 2:
                                if (Exists)
                                {
                                    newCells.TryAdd(new CellCoords(x, y), CurrentCell);
                                }
                                break;
                            case 3:
                                if (Exists == false)
                                {
                                    bool Added = newCells.TryAdd(new CellCoords(x, y), CurrentCell);
                                    if (Added)
                                    {
                                        //TODO: increase heat+
                                    }
                                }
                                else
                                {
                                    newCells.TryAdd(new CellCoords(x, y), CurrentCell);
                                }
                                break;
                            default:
                                if (Exists)
                                {
                                    //TODO: decrease heat-
                                }
                                break;
                        }
                    }
                }
            }

            cells = newCells;
        }

        private int GetAliveNeighbours(int CoordX, int CoordY)
        {
            int Neighbours = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
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