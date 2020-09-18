
using System;
using System.Collections.Generic;

namespace Conway
{
    public class ConwayGame
    {
        private readonly int _defaultProbability = 50;
        private readonly int _defaultIterations = 50;
        private readonly int _defaultStarterSize = 20;
        private readonly int _defaultNumOfSimulations = 1;
        private readonly int _checkAfterIterations = 100;
        private readonly int _checkForIterations = 5;

        private Dictionary<CellCoords, Cell> _cells;
        private List<IterationStats> _stats;
        private int _currentIteration;
        private List<Dictionary<CellCoords, Cell>> _repetitionList;

        public ConwayGame()
        {
            _cells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
            _stats = new List<IterationStats>();
            _repetitionList = new List<Dictionary<CellCoords, Cell>>();
        }

        public void Run(InputCsvFile input)
        {
            int fieldSize = input.FieldSize;
            int probability = input.ProbabilityForLife;
            int numberOfIterations = input.NumberOfIterations;
            int numberOfSimulations = input.NumberOfSimulations;
            bool writeStats = input.SaveStatistics;
            string statsName = input.NameStatisticFile;
            if (fieldSize == -1) fieldSize = _defaultStarterSize;
            if (probability == -1) probability = _defaultProbability;
            if (numberOfIterations == -1) numberOfIterations = _defaultIterations;
            if (numberOfSimulations == -1) numberOfSimulations = _defaultNumOfSimulations;

            for (int simulation = 0; simulation < numberOfSimulations; simulation++)
            {
                Console.WriteLine("--------------------------");
                _cells.Clear();
                _repetitionList.Clear();
                _stats.Clear();
                GenerateCellsWithProbability(probability, fieldSize);
                Console.WriteLine($"Generated Cells: {_cells.Count}");
                Console.WriteLine($"Field Size: {fieldSize * 2} x {fieldSize * 2}");
                int checkStarted = 0;
                Boolean iterationRepeated = false;
                for (int i = 0; i < numberOfIterations; i++)
                {
                    _currentIteration = i + 1;
                    IterateSimulation();

                    if (i % _checkAfterIterations == 0)
                    {
                        checkStarted = i;
                    }
                    if (checkStarted != 0)
                    {

                        if (i - checkStarted <= _checkForIterations)
                        {
                            if (IsCurrentIterationRepetition())
                            {
                                iterationRepeated = true;
                                Console.WriteLine($"Iterations: {i + 1}");
                                break;
                            }
                            AddDictionaryToCheckList();
                        }
                        else if (i - checkStarted == _checkForIterations + 1)
                        {
                            _repetitionList.Clear();
                        }
                    }

                }
                if (iterationRepeated)
                {
                    Console.WriteLine("Field repeated - simulation terminated");
                }
                else
                {
                    Console.WriteLine($"Alive after 1000 iterations: {_cells.Count}");
                }

                if (writeStats)
                {
                    if (iterationRepeated)
                    {
                        IterationStats.WriteStats(_stats, $"./{ statsName }-{ simulation }-terminated.csv");
                    }
                    else
                    {
                        IterationStats.WriteStats(_stats, $"./{ statsName }-{ simulation }.csv");
                    }
                }
            }
            Console.WriteLine("--------------------------");
        }

        private void AddDictionaryToCheckList()
        {
            Dictionary<CellCoords, Cell> copyDictionary = new Dictionary<CellCoords, Cell>(_cells.Count, _cells.Comparer);
            foreach (KeyValuePair<CellCoords, Cell> cellPair in _cells)
            {
                copyDictionary.Add(cellPair.Key, cellPair.Value);
            }

            _repetitionList.Add(copyDictionary);
        }

        private Boolean IsCurrentIterationRepetition()
        {

            foreach (Dictionary<CellCoords, Cell> iterateDictionary in _repetitionList)
            {
                Boolean iterateDictionaryRepeated = true;
                if (iterateDictionary.Count == _cells.Count)
                {
                    foreach (CellCoords cellCoord in iterateDictionary.Keys)
                    {
                        Cell currentAliveCell;
                        if (!_cells.TryGetValue(cellCoord, out currentAliveCell))
                        {
                            iterateDictionaryRepeated = false;
                            break;
                        }
                        return iterateDictionaryRepeated;
                    }

                }

            }
            return false;
        }

        private void GenerateDefinedAmountOfCells(int amountOfCells, int fieldSize)
        {
            for (int i = 0; i < amountOfCells; i++)
            {
                bool added = false;
                do
                {
                    var Random = new System.Random();
                    var x = Random.Next(fieldSize * -1, fieldSize + 1);
                    var y = Random.Next(fieldSize * -1, fieldSize + 1);
                    added = _cells.TryAdd(new CellCoords(x, y), new Cell());
                } while (added == false);
            }
        }

        private void GenerateCellsWithProbability(int promilleMax, int fieldSize)
        {
            Random Random = new System.Random();
            for (int x = fieldSize * -1; x <= fieldSize; x++)
            {
                for (int y = fieldSize * -1; y <= fieldSize; y++)
                {

                    int random = Random.Next(1001);
                    if (random <= promilleMax)
                    {
                        _cells.Add(new CellCoords(x, y), new Cell());
                    }
                }
            }
        }

        private void IterateSimulation()
        {
            var newCells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
            var deadCells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
            var stat = new IterationStats();
            stat.Iteration = _currentIteration;
            stat.CellCount = _cells.Count;
            foreach (System.Collections.Generic.KeyValuePair<CellCoords, Cell> cell in _cells)
            {
                for (int y = cell.Key.y - 1; y <= (cell.Key.y + 1); y++)
                {
                    for (int x = cell.Key.x - 1; x <= (cell.Key.x + 1); x++)
                    {
                        int neighbours = GetAliveNeighbours(x, y);
                        Cell currentCell;
                        bool exists = _cells.TryGetValue(new CellCoords(x, y), out currentCell);
                        switch (neighbours)
                        {
                            case 2:
                                if (exists)
                                {
                                    newCells.TryAdd(new CellCoords(x, y), currentCell);
                                }
                                break;
                            case 3:
                                if (exists)
                                {
                                    newCells.TryAdd(new CellCoords(x, y), currentCell);
                                }
                                else
                                {
                                    bool Added = newCells.TryAdd(new CellCoords(x, y), currentCell);
                                    if (Added)
                                    {
                                        stat.PositiveHeat++;
                                    }
                                }
                                break;
                            default:
                                if (exists)
                                {
                                    bool Added = deadCells.TryAdd(new CellCoords(x, y), currentCell);
                                    if (Added)
                                    {
                                        stat.NegativeHeat++;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            _stats.Add(stat);
            _cells = newCells;
        }

        private int GetAliveNeighbours(int coordX, int coordY)
        {
            int neighbours = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        Cell dummy;
                        bool alive;
                        alive = _cells.TryGetValue(new CellCoords(coordX + x, coordY + y), out dummy);
                        if (alive)
                        {
                            neighbours++;
                        }
                    }
                }
            }
            return neighbours;
        }
    }
}