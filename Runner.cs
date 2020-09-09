using System;
using System.IO;
using conway.lib;
using System.Collections.Generic;

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
            runList = new List<List<int>>();
            if (args.Length > 0)
            {
                String fileLocation = args[0];
                StreamReader reader = new StreamReader(fileLocation);

                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    String[] values = line.Split(";");
                    if (int.TryParse(values[0], out int n))
                    {
                        List<int> valueList = new List<int>();
                        foreach (String value in values)
                        {
                            int addNumber;
                            if (int.TryParse(value, out addNumber))
                            {
                                valueList.Add(addNumber);
                            }
                        }
                        runList.Add(valueList);
                    }
                }
            }
            ConwayGame conwayGame = new ConwayGame(20);
            foreach (List<int> valueList in runList)
            {
                while (valueList.Count < 4)
                {
                    valueList.Add(-1);
                }
                conwayGame.Run(valueList[_indexOfFieldSize], valueList[_indexOfSpawnChance], valueList[_indexOfIterationsNumber], valueList[_indexOfFieldSizeSimulationsNumber]);
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
}
