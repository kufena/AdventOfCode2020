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

            SortedDictionary<int, Image> images = new SortedDictionary<int, Image>();
            line = sr.ReadLine();
            while (line != null)
            {
                // Tile <num>:
                var numstr = line.Trim().Substring(5, line.Trim().Length - 6);
                int num = Int32.Parse(numstr);
                WriteLine($"Getting image {num}");
                // ten lines:
                string[] image = new string[10];
                for (int i = 0; i < 10; i++)
                {
                    line = sr.ReadLine().Trim();
                    image[i] = line;
                }

                Image im = new Image(image, num);
                images.Add(num, im);

                // blank
                line = sr.ReadLine();
                line = sr.ReadLine();
            }

            Console.WriteLine($"Found {images.Count} images.");
            foreach (var k1 in images.Keys)
            {
                foreach (var k2 in images.Keys)
                {
                    if (k2 != k1)
                    {
                        byte n = images[k1].matchImage(images[k2]);
                        if (n > 0)
                        {
                            WriteLine($"{k1} matches {k2} so adding {k1} as vertes to {k2} and {k2} as a vertex to {k1}");
                            images[k1].AddVertex(images[k2]);
                            images[k2].AddVertex(images[k1]);
                        }
                    }
                }
            }

            long sum = 1;
            Image topleft = null;

            foreach (var (n, k) in images)
            {
                if (k.NumVertices <= 2)
                { // ToDo
                    sum = sum * k.num;
                    WriteLine($"One corner is {k}");
                }
            }

            // for test expect 20899048083289 got 20899048083289
            WriteLine($"sum is {sum}");

            // Part2.

            List<Image> corners = new List<Image>();
            foreach (var (n, k) in images)
            {
                if (k.NumVertices == 2)
                {
                    corners.Add(k);
                }
            }

            foreach (var k in corners)
            {
                WriteLine($"{k.num} is a corner.");
                if (k.TopLeft())
                {
                    topleft = k;
                    WriteLine($"{k.num} is now top left!");
                    break;
                }
            }

            if (topleft == null)
            {
                WriteLine("No top left found");
                return;
            }

            Stack<Image> remaining = new Stack<Image>();

            remaining.Push(topleft.finalright);
            remaining.Push(topleft.finaldown);

            while(remaining.Count > 0)
            {
                var img = remaining.Pop();
                if(img.matchThese())
                {
                    if (img.finalright != null) remaining.Push(img.finalright);
                    if (img.finaldown != null) remaining.Push(img.finaldown);
                }
            }

            Image bot;
            var imgg = topleft;
            int size = (int) Math.Sqrt(images.Count);
            for (int i = 0; i < (size+1); i++)
            {
                bot = imgg.finaldown;
                for (int j = 0; j < (size+1); j++)
                {
                    WriteLine($"({i},{j}) is {imgg.num}");
                    if (imgg.finalright == null)
                        break;
                    imgg = imgg.finalright;
                }
                imgg = bot;
            }
        }
    }
}
