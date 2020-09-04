using System;
using conway.lib;

namespace conway
{
    class Runner
    {
        private static int StarterSize = new Int32();
        private static int MaxStarterSize = 50;
        static void Main(string[] args)
        {
            do
            {
              Console.WriteLine("Field size? "); 
              StarterSize = int.Parse(Console.ReadLine());
            } while (StarterSize < 1 || StarterSize > MaxStarterSize );
            ConwayGame conway = new ConwayGame(StarterSize);
            conway.Run();
        }
    }
}
