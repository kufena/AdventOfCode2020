using System;
using System.IO;

namespace _17
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello let's get going on energy planes.");
            var f = File.OpenRead(args[0]);
            var sr = new StreamReader(f);

            // There's a sneaky trick in this one.
            // The test input given does something really funny - the phases don't show
            // the same position within the space.
            // Essentially - you can say the final space is going to have dimensions twice the
            // number of iterations plus the size of the dimension in the test.  So an eight by
            // eight 1-D test, for six iterations should eventually have size 20x20x21.
            // BUT IT DOESN'T!
            // Like any game-of-life type scenario, the active part moves around, AND THIS IS NOT
            // SHOWN IN THE EXAMPLE/TEST.
            // To fix this, I fix the code to six iterations, but the board size is for 12 iterations,
            // because I guess it'll never move more than one square in any direction at a time.
            
            //Part1.Part1action(sr);
            Part2.Part2action(sr);
        }
    }
}
