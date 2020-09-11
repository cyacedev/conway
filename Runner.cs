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
        private static int _indexOfFieldSize = 0;
        private static int _indexOfSpawnChance = 1;
        private static int _indexOfIterationsNumber = 2;
        private static int _indexOfFieldSizeSimulationsNumber = 3;
        private static int StarterSize = new Int32();
        private static int MaxStarterSize = 50;
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
                
                foreach(InputCsvFile csvTest in records){
                    Console.WriteLine($"input Data: \nsize({csvTest.FieldSize}), prob({csvTest.ProbabilityForLife}), it({csvTest.NumberOfIterations}), sim({csvTest.NumberOfSimulations})");
                    conwayGame.Run(csvTest.FieldSize, csvTest.ProbabilityForLife, csvTest.NumberOfIterations, csvTest.NumberOfSimulations);
                }

            return;
            do
            {
                Console.WriteLine("Field size (in every direction)? ");
                try
                {
                    StarterSize = int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    StarterSize = 0;
                }
            } while (StarterSize < 1);
            ConwayGame conway = new ConwayGame(StarterSize);
            conway.Run(-1, -1, -1, -1);
        }


    }

    public class InputCsvFile
    {
        public int FieldSize { get; set; }
        public int ProbabilityForLife{get;set;}
        public int NumberOfIterations{get;set;}
        public int NumberOfSimulations{get;set;}
    }
}
