using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;

namespace _20
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string line;
            var f = File.OpenRead(args[0]);
            StreamReader sr = new StreamReader(f);

            SortedDictionary<int,Image> images = new SortedDictionary<int, Image>();
            line = sr.ReadLine();
            while(line != null) {
                // Tile <num>:
                var numstr = line.Trim().Substring(5,line.Trim().Length-6);
                int num = Int32.Parse(numstr);
                WriteLine($"Getting image {num}");
                // ten lines:
                string[] image = new string[10];
                for(int i = 0; i < 10; i++) {
                    line = sr.ReadLine().Trim();
                    image[i] = line;
                }

                Image im = new Image(image, num);
                images.Add(num,im);

                // blank
                line = sr.ReadLine();
                line = sr.ReadLine();
            }

            Console.WriteLine($"Found {images.Count} images.");
            foreach(var k1 in images.Keys) {
                foreach(var k2 in images.Keys) {
                    if (k2 != k1) {
                        byte n = images[k1].matchImage(images[k2]);
                        if (n > 0) {
                            WriteLine($"{k1} matches {k2} so adding {k1} as vertes to {k2} and {k2} as a vertex to {k1}");
                            images[k1].AddVertex(images[k2]);
                            images[k2].AddVertex(images[k1]);
                        }
                    }
                }
            }

            long sum = 1;
            Image topleft = null;

            foreach(var (n,k) in images) {
                if (k.NumVertices <= 2) { // ToDo
                    sum = sum * k.num;
                    WriteLine($"One corner is {k}");
                }
            }

            // for test expect 20899048083289 got 20899048083289
            WriteLine($"sum is {sum}");

            foreach(var (n,k) in images) {
                Console.WriteLine($"Orientating {n}");
                k.Orientate();
            }
        }
    }
}
