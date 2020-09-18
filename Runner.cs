using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Conway
{
    class Runner
    {
        private static List<List<int>> runList;
        static void Main(string[] args)
        {
            ConwayGame conwayGame = new ConwayGame();
            runList = new List<List<int>>();
            if (args.Length > 0)
            {
                String fileLocation = args[0];
                StreamReader reader = new StreamReader(fileLocation);
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                IEnumerable<InputCsvFile> records = csv.GetRecords<InputCsvFile>();

                foreach (InputCsvFile record in records)
                {
                    Console.WriteLine($"input Data: \nsize({record.FieldSize}), prob({record.ProbabilityForLife}), it({record.NumberOfIterations}), sim({record.NumberOfSimulations})");
                    conwayGame.Run(record);
                }

                return;
            }
        }


    }

    public class InputCsvFile
    {
        public int FieldSize { get; set; }
        public int ProbabilityForLife { get; set; }
        public int NumberOfIterations { get; set; }
        public int NumberOfSimulations { get; set; }

        [BooleanTrueValues("true")]
        [BooleanFalseValues("false")]
        public bool SaveStatistics { get; set; }

        public string NameStatisticFile { get; set; }
    }
}
