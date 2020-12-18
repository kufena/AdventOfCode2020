using System;
using System.IO;
using System.Collections.Generic;

using static System.Console;

namespace _18
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = File.OpenRead(args[0]);
            StreamReader sr = new StreamReader(f);

            long acc = 0;
            string line;

            while ((line = sr.ReadLine()) != null) {
                //acc += Parser.Parse(line);
                acc += Parser.ParsePart2(line);
            }
            
            Console.WriteLine($"Final value is {acc}");
        }
    }
}
