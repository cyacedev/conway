  
using System;
using System.Collections.Generic;

namespace conway.lib
{
    public class ConwayGame
    {
        private readonly int StarterSize;
        private Dictionary<CellCoords, Cell> cells;

        private int CheckAfterIterations = 100;
        private int CheckForIterations = 5;

        private List<Dictionary<CellCoords, Cell>> RepetitionList;

        public ConwayGame(int StarterSize)
        {
            this.StarterSize = StarterSize;
            cells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
        }

        public void Run()
        {
            RepetitionList = new List<Dictionary<CellCoords, Cell>>();
            GenerateCellsWithProbability(50);
            Console.WriteLine($"Generated Cells: {cells.Count}");
            Console.WriteLine($"Field Size: {StarterSize * 2} x {StarterSize * 2}");
            int checkStarted = 0;
            Boolean iterationRepeated = false;
            for (int i = 0; i < 1000; i++)
            {
                IterateSimulation();
            
                if (i % CheckAfterIterations == 0)
                {
                    checkStarted = i;
                }
                if (i - checkStarted <= CheckForIterations)
                {
                    if (IsCurrentIterationRepetition())
                    {
                        iterationRepeated = true;
                        Console.WriteLine($"Iterations: {i}");
                        break;
                    }
                    AddDictionaryToCheckList();
                }
                else if (i - checkStarted == CheckForIterations + 1)
                {
                    RepetitionList.Clear();
                }
            
            }
            if(iterationRepeated){
                //fuck you warning
            }
            /*if (iterationRepeated)
            {
                foreach (Dictionary<CellCoords, Cell> dictToPrint in RepetitionList)
                {
                    Console.WriteLine("previous dictionary printed-----------------------------------------------------------------");
                    foreach (KeyValuePair<CellCoords, Cell> cell in dictToPrint)
                    {
                        Console.WriteLine($"Alive: x({cell.Key.x}), y({cell.Key.y})");
                    }
                }
                Console.WriteLine("final dictionary printed-------------------------------------------------------------------------");
                foreach (KeyValuePair<CellCoords, Cell> cell in cells)
                {
                    Console.WriteLine($"Alive: x({cell.Key.x}), y({cell.Key.y})");
                }
            }
            foreach (KeyValuePair<CellCoords, Cell> cell in cells)
            {
                Console.WriteLine($"Alive: x({cell.Key.x}), y({cell.Key.y})");
            }
            */

            Console.WriteLine($"Alive after 1000 iterations: {cells.Count}");
        }

        private void AddDictionaryToCheckList()
        {
            Dictionary<CellCoords, Cell> copyDictionary = new Dictionary<CellCoords, Cell>(cells.Count, cells.Comparer);
            foreach (KeyValuePair<CellCoords, Cell> cellPair in cells)
            {
                copyDictionary.Add(cellPair.Key, cellPair.Value);
            }

            this.RepetitionList.Add(copyDictionary);
        }

        private Boolean IsCurrentIterationRepetition()
        {
            foreach (Dictionary<CellCoords, Cell> iterateDictionary in RepetitionList)
            {
                Boolean iterateDictionaryRepeated = true;
                if (iterateDictionary.Count == cells.Count)
                {
                    foreach (CellCoords cellCoord in iterateDictionary.Keys)
                    {
                        Cell currentAliveCell;
                        if (!cells.TryGetValue(cellCoord, out currentAliveCell))
                        {
                            iterateDictionaryRepeated = false;
                            break;
                        }
                    }
                    if (iterateDictionaryRepeated)
                    {
                        Console.WriteLine("FOUND DICTIONARY-------------------------");
                        PrintDictionary(iterateDictionary);
                        Console.WriteLine("CURRENT DICTIONARY-------------------------");
                        PrintDictionary(cells);
                        return true;
                    }
                }

            }
            return false;
        }

        private void PrintDictionary(Dictionary<CellCoords, Cell> dictionary){
            foreach (KeyValuePair<CellCoords, Cell> cell in dictionary)
                    {
                        Console.WriteLine($"Alive: x({cell.Key.x}), y({cell.Key.y})");
                    }
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
            Random Random = new System.Random();
            for (int x = StarterSize * -1; x <= StarterSize; x++)
            {
                for (int y = StarterSize * -1; y <= StarterSize; y++)
                {

                    int random = Random.Next(1001);
                    if (random <= PromilleMax)
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