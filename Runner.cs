using System;
using conway.lib;

namespace conway
{
    class Runner
    {
        private static int PlayfieldSize = new Int32();
        static void Main(string[] args)
        {
            do
            {
              Console.WriteLine("Field size? "); 
              PlayfieldSize = int.Parse(Console.ReadLine());
            } while (PlayfieldSize == 0 || PlayfieldSize > 50 );
            ConwayGame conway = new ConwayGame(PlayfieldSize);
        }
    }
}
