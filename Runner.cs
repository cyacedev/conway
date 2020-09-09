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
            conway.Run();
        }
    }
}
