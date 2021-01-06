using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using CsvHelper;

namespace Conway
{
    public class IterationStats
    {
        public int Iteration { get; set; } = 0;
        public int PositiveHeat { get; set; } = 0;
        public int NegativeHeat { get; set; } = 0;
        public int CellCount { get; set; } = 0;
        public float IterationAverageNeighbours { get; set; } = 0;
        public float IterationDensity { get; set; } = 0;

        public static void WriteStats(List<IterationStats> stats, string path)
        {

            using (var writer = new System.IO.StreamWriter(path))
            using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(stats);
            }
        }

        public static void AddAverageStats(List<IterationStats> newStats, string path)
        {

            List<IterationStats> previousStats = new List<IterationStats>();
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                var previousStatsReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                IEnumerable<IterationStats> statsRecords = previousStatsReader.GetRecords<IterationStats>();
                foreach (IterationStats addStat in statsRecords)
                {
                    previousStats.Add(addStat);
                }
                reader.Close();

                if (newStats.Count == previousStats.Count)
                {
                    for (int i = 0; i < previousStats.Count; i++)
                    {

                        IterationStats adjustStat = previousStats[i];
                        IterationStats addStat = newStats[i];
                        if (addStat.Iteration != adjustStat.Iteration)
                        {
                            Console.WriteLine("Different iteration number");
                        }
                        adjustStat.PositiveHeat += addStat.PositiveHeat;
                        adjustStat.NegativeHeat += addStat.NegativeHeat;
                        adjustStat.CellCount += addStat.CellCount;
                        adjustStat.IterationAverageNeighbours += addStat.IterationAverageNeighbours;
                        adjustStat.IterationDensity += addStat.IterationDensity;
                    }
                    WriteStats(previousStats, path);
                }
                else
                {
                    Console.WriteLine("Not same size of iteration stats");
                }

            }
            else
            {
                WriteStats(newStats, path);
            }
        }

        public static void CalculateAndWriteAverageStats(int simulationNumber, string readPath, string writePath)
        {
            if (File.Exists(readPath))
            {
                List<IterationStats> averageStats = new List<IterationStats>();
                StreamReader reader = new StreamReader(readPath);
                var previousStatsReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                IEnumerable<IterationStats> statsRecords = previousStatsReader.GetRecords<IterationStats>();
                foreach (IterationStats calculatedStat in statsRecords)
                {
                    IterationStats averageStat = new IterationStats();
                    averageStat.Iteration = calculatedStat.Iteration;
                    averageStat.PositiveHeat = calculatedStat.PositiveHeat / simulationNumber;
                    averageStat.NegativeHeat = calculatedStat.NegativeHeat / simulationNumber;
                    averageStat.CellCount = calculatedStat.CellCount / simulationNumber;
                    averageStat.IterationAverageNeighbours = calculatedStat.IterationAverageNeighbours / simulationNumber;
                    averageStat.IterationDensity = calculatedStat.IterationDensity / simulationNumber;

                    averageStats.Add(averageStat);
                }
                reader.Close();
                WriteStats(averageStats, writePath);
            }
            File.Delete(readPath);
        }
    }

    public class IterationEndPosition
    {
        public static void WriteEndPosition(Dictionary<CellCoords, Cell> writeDictionary, string path)
        {
            List<PredefinedPosition> endPositions = new List<PredefinedPosition>();
            foreach (KeyValuePair<CellCoords, Cell> cellEntry in writeDictionary)
            {
                endPositions.Add(new PredefinedPosition(cellEntry.Key.x, cellEntry.Key.y));
            }
            using (var writer = new System.IO.StreamWriter(path))
            using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(endPositions);
            }
        }
    }
}