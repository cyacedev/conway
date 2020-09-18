using System.Collections.Generic;

namespace Conway
{
    public class IterationStats
    {
        public int Iteration { get; set; }
        public int PositiveHeat { get; set; }
        public int NegativeHeat { get; set; }
        public int CellCount { get; set; }

        public static void WriteStats(List<IterationStats> stats, string path)
        {
            using (var writer = new System.IO.StreamWriter(path))
            using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(stats);
            }
        }
    }
}