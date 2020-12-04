
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
        private List<ChangedCellCoords> _previouslyChangedCoords;
        private List<ChangedCellCoords> _newChangedCoords;
        
        private List<IterationStats> _stats;
        private int _currentIteration;
        private List<Dictionary<CellCoords, Cell>> _repetitionList;

        public ConwayGame()
        {
            _cells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
            _previouslyChangedCoords = new List<ChangedCellCoords>();
            _newChangedCoords = new List<ChangedCellCoords>();
            _stats = new List<IterationStats>();
            _repetitionList = new List<Dictionary<CellCoords, Cell>>();
        }

        public void RunPredefinedGame(IEnumerable<PredefinedPosition> livingCells, InputCsvFile input)
        {
            _cells.Clear();
            _stats.Clear();
            _repetitionList.Clear();
            setPredefinedCells(livingCells);
            Run(input, true);

        }

        private void setPredefinedCells(IEnumerable<PredefinedPosition> livingCells)
        {
            foreach (PredefinedPosition newCell in livingCells)
            {
                _cells.TryAdd(new CellCoords(newCell.x, newCell.y), new Cell());
            }
        }


        public void Run(InputCsvFile input, bool predefinedPosition)
        {
            int fieldSize = input.FieldSize;
            int probability = input.ProbabilityForLife;
            int numberOfIterations = input.NumberOfIterations;
            int numberOfSimulations = input.NumberOfSimulations;
            bool writeStats = input.SaveStatistics;
            string statsName = input.NameStatisticFile;
            string endStateName = input.NameEndStateFile;
            if (fieldSize == -1) fieldSize = _defaultStarterSize;
            if (probability == -1) probability = _defaultProbability;
            if (numberOfIterations == -1) numberOfIterations = _defaultIterations;
            if (numberOfSimulations == -1) numberOfSimulations = _defaultNumOfSimulations;
            if (predefinedPosition) numberOfSimulations = 1; //only the predefined game has to be run
            for (int simulation = 0; simulation < numberOfSimulations; simulation++)
            {
                Console.WriteLine("--------------------------");
                if (!predefinedPosition)
                {
                    _cells.Clear();
                }
                _repetitionList.Clear();
                _stats.Clear();
                if (!predefinedPosition)
                {
                    GenerateCellsWithProbability(probability, fieldSize);
                    Console.WriteLine($"Generated Cells: {_cells.Count}");
                    Console.WriteLine($"Field Size: {fieldSize} x {fieldSize}");
                }
                else
                {
                    Console.WriteLine($"Predefined Generated Cells: {_cells.Count}");
                }
                FillChangedPositionsOnStart();
                int checkStarted = 0;
                bool iterationRepeated = false;
                bool iterationDead = false;
                for (int i = 0; i < numberOfIterations; i++)
                {
                    _currentIteration = i + 1;
                    IterateSimulation();
                    //Console.WriteLine($"Current iteration: {i}");
                    if(_cells.Count == 0){
                        iterationDead = true;
                        break;
                    }

                    //For average stats, repetition is not enabled
                    if(!input.AverageStats){
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
                }
                if (iterationRepeated)
                {
                    Console.WriteLine("Field repeated - simulation terminated");
                }
                else if(iterationDead)
                {
                    Console.WriteLine("Field dead - simulation terminated");
                }
                else
                {
                    Console.WriteLine($"Alive after 1000 iterations: {_cells.Count}");
                }

                if (writeStats)
                {

                    IterationStats endStats = new IterationStats();
                    endStats.CellCount = _cells.Count;
                    _stats.Add(endStats);
                    if (iterationRepeated)
                    {
                        IterationStats.WriteStats(_stats, $"./{ statsName }-{ simulation }-terminated.csv");
                    }
                    else
                    {
                        IterationStats.WriteStats(_stats, $"./{ statsName }-{ simulation }.csv");
                        
                        if(input.AverageStats){
                            IterationStats.AddAverageStats(_stats, $"./{ statsName }-cache.csv");    
                        }
                        
                    }

                }
                if (input.SaveEndState)
                {
                    IterationEndPosition.WriteEndPosition(_cells, $"./{ endStateName }-{ simulation }.csv");
                }
                
            }
            if(input.AverageStats){
                IterationStats.CalculateAndWriteAverageStats(input.NumberOfSimulations, $"./{ statsName }-cache.csv", $"./{ statsName }-average.csv");
            }
            Console.WriteLine("--------------------------");
        }

        private void AddDictionaryToCheckList()
        {
            Dictionary<CellCoords, Cell> copyDictionary = new Dictionary<CellCoords, Cell>(_cells.Count, _cells.Comparer);
            foreach (KeyValuePair<CellCoords, Cell> cellPair in _cells)
            {
                copyDictionary.Add(new CellCoords(cellPair.Key.x, cellPair.Key.y), new Cell());
            }

            _repetitionList.Add(copyDictionary);
        }

        private Boolean IsCurrentIterationRepetition()
        {

            foreach (Dictionary<CellCoords, Cell> iterateDictionary in _repetitionList)
            {
                bool iterateDictionaryRepeated = true;
                if (iterateDictionary.Count == _cells.Count)
                {
                    foreach (CellCoords cellCoord in iterateDictionary.Keys)
                    {
                        if (!_cells.ContainsKey(cellCoord))
                        {
                            iterateDictionaryRepeated = false;
                            break;
                        }
                    }
                    if(iterateDictionaryRepeated){
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
            
            fieldSize--;
            //Fieldsize 1 generates 1 x 1 field (because 0x0 is also generated)
            int negativStartPoint = 0;
            int positiveEndPoint = 0;
            if (fieldSize % 2 == 0)
            {
                negativStartPoint = fieldSize / 2 * -1;
                positiveEndPoint = fieldSize / 2;
            }
            else
            {
                negativStartPoint = fieldSize / 2 * -1;
                positiveEndPoint = fieldSize / 2 + 1;
            }
            Random Random = new System.Random();
            for (int x = negativStartPoint; x <= positiveEndPoint; x++)
            {
                for (int y = negativStartPoint; y <= positiveEndPoint; y++)
                {

                    int random = Random.Next(1001);
                    if (random <= promilleMax)
                    {
                        _cells.Add(new CellCoords(x, y), new Cell());
                    }
                }
            }
        }

        private void FillChangedPositionsOnStart(){
            this._newChangedCoords = new List<ChangedCellCoords>();
            foreach(KeyValuePair<CellCoords, Cell> cell in _cells){
                this._newChangedCoords.Add(new ChangedCellCoords(cell.Key, true));
            }
        }

        private void IterateSimulation()
        {
            
            this._previouslyChangedCoords = new List<ChangedCellCoords>(this._newChangedCoords);
            this._newChangedCoords.Clear();
            Dictionary<CellCoords, Cell> checkedCells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
            CellAverageNeighbours averageNeighbours = new CellAverageNeighbours();
            CellDensity iterationDensity = new CellDensity(_cells.Count);
            //var newCells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
            var deadCells = new Dictionary<CellCoords, Cell>(new CellCoordsComparer());
            var stat = new IterationStats();
            stat.Iteration = _currentIteration;
            foreach(KeyValuePair<CellCoords, Cell> cell in _cells){
                iterationDensity.AddCellCoordToCheck(cell.Key);
            }
            iterationDensity.CalculateDensity();
            stat.IterationDensity = iterationDensity.cellDensity;
            stat.CellCount = _cells.Count;
            stat.PositiveHeat = 0;
            stat.NegativeHeat = 0;
            foreach (ChangedCellCoords changedCellCoords in this._previouslyChangedCoords)
            {
                for (int y = changedCellCoords.cellCoords.y - 1; y <= (changedCellCoords.cellCoords.y + 1); y++)
                {
                    for (int x = changedCellCoords.cellCoords.x - 1; x <= (changedCellCoords.cellCoords.x + 1); x++)
                    {
                        if (!checkedCells.TryAdd(new CellCoords(x, y), new Cell()))
                        {
                            continue;
                        }
                        int neighbours = GetAliveNeighbours(x, y);
                        Cell currentCell;
                        bool exists = _cells.TryGetValue(new CellCoords(x, y), out currentCell);

                        switch (neighbours)
                        {
                            case 2:
                                //dead cells remain dead, living cells remain living (no actions)
                                break;
                            case 3:
                                //if living cell, remains living
                                if (!exists)
                                {
                                    this._newChangedCoords.Add(new ChangedCellCoords(new CellCoords(x,y),true));
                                    stat.PositiveHeat++;
                                }
                                break;
                            default:
                                if (exists)
                                {
                                    this._newChangedCoords.Add(new ChangedCellCoords(new CellCoords(x,y),false));
                                    stat.NegativeHeat++;
                                }
                                break;
                        }
                    }
                }
            }

            int addedCells = 0;
            int removedCells = 0;
            foreach(ChangedCellCoords changedCoords in this._newChangedCoords){
                if(changedCoords.newLife){
                    _cells.Add(changedCoords.cellCoords, new Cell());
                    addedCells++;
                }else{
                    _cells.Remove(changedCoords.cellCoords);
                    removedCells++;
                }
            }

            stat.IterationAverageNeighbours = averageNeighbours.averageNeighbourNumber;
            checkedCells.Clear();
            checkedCells = null;
            _stats.Add(stat);
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

    public class PredefinedPosition
    {
        public int x { get; set; }
        public int y { get; set; }

        public PredefinedPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public PredefinedPosition()
        {

        }
    }
}