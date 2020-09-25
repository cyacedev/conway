using System.Collections.Generic;

namespace Conway
{
    public class IterationStats
    {
        public int Iteration { get; set; }
        public int PositiveHeat { get; set; }
        public int NegativeHeat { get; set; }
        public int CellCount { get; set; }
        public float IterationAverageNeighbours { get; set; }
        public float IterationDensity { get; set; }

        public static void WriteStats(List<IterationStats> stats, string path)
        {

            using (var writer = new System.IO.StreamWriter(path))
            using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(stats);
            }
        }
    }

    public class IterationEndPosition{
        public static void WriteEndPosition(Dictionary<CellCoords, Cell> writeDictionary, string path){
            List<PredefinedPosition> endPositions = new List<PredefinedPosition>();
            foreach(KeyValuePair<CellCoords, Cell> cellEntry in writeDictionary){
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