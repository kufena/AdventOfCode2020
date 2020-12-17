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

            Part2.Part2action(sr);
        }
    }
}
