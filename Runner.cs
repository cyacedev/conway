using System;
using System.Globalization;
using System.IO;
using conway.lib;
using System.Collections.Generic;
using CsvHelper;
namespace conway
{
    class Runner
    {
        private static List<List<int>> runList;
        static void Main(string[] args)
        {
            ConwayGame conwayGame = new ConwayGame(20);
            runList = new List<List<int>>();
            if (args.Length > 0)
            {
                String fileLocation = args[0];
                StreamReader reader = new StreamReader(fileLocation);
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                IEnumerable<InputCsvFile> records = csv.GetRecords<InputCsvFile>();

                foreach (InputCsvFile csvTest in records)
                {
                    Console.WriteLine($"input Data: \nsize({csvTest.FieldSize}), prob({csvTest.ProbabilityForLife}), it({csvTest.NumberOfIterations}), sim({csvTest.NumberOfSimulations})");
                    conwayGame.Run(csvTest.FieldSize, csvTest.ProbabilityForLife, csvTest.NumberOfIterations, csvTest.NumberOfSimulations);
                }

                return;
            }
        }

        public class InputCsvFile
        {
            public int FieldSize { get; set; }
            public int ProbabilityForLife { get; set; }
            public int NumberOfIterations { get; set; }
            public int NumberOfSimulations { get; set; }
        }
    }
}
