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
                String conditionFileLocation = args[0];
                StreamReader reader = new StreamReader(conditionFileLocation);
                var csvConditions = new CsvReader(reader, CultureInfo.InvariantCulture);

                if (args.Length > 1)
                {
                    IEnumerable<InputCsvFile> conditionRecords = csvConditions.GetRecords<InputCsvFile>();
                    InputCsvFile firstEntry = new InputCsvFile();
                    using (IEnumerator<InputCsvFile> enumer = conditionRecords.GetEnumerator())
                    {
                        if (enumer.MoveNext()) firstEntry = enumer.Current;
                    }
                    string predefinedInputLocation = args[1];
                    StreamReader predefinedInputReader = new StreamReader(predefinedInputLocation);
                    var csvPredefinedPositions = new CsvReader(predefinedInputReader, CultureInfo.InvariantCulture);
                    IEnumerable<PredefinedPosition> predefinedCellRecords = csvPredefinedPositions.GetRecords<PredefinedPosition>();
                    conwayGame.RunPredefinedGame(predefinedCellRecords, firstEntry);
                }
                else
                {
                    IEnumerable<InputCsvFile> records = csvConditions.GetRecords<InputCsvFile>();

                    foreach (InputCsvFile record in records)
                    {
                        Console.WriteLine($"input Data: \nsize({record.FieldSize}), prob({record.ProbabilityForLife}), it({record.NumberOfIterations}), sim({record.NumberOfSimulations})");
                        conwayGame.Run(record, false);
                    }
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
        public bool SaveEndState { get; set; }
        public bool AverageStats{ get; set; }

        public string NameStatisticFile { get; set; }
        public string NameEndStateFile { get; set; }
    }
}
