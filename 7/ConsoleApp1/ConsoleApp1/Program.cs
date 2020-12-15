using System;
using System.Collections.Generic;
using System.IO;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = File.OpenRead(@"e:\work\adventofcode\7\ConsoleApp1\ConsoleApp1\input.txt");
            StreamReader sr = new StreamReader(f);

            Dictionary<string, Bag> bags = new Dictionary<string, Bag>();
            string line;

            while (!((line = sr.ReadLine()) is null))
            {
                var splits = line.Split(' ', StringSplitOptions.None);
                string bagname = splits[0] + " " + splits[1];
                Bag b;
                if (bags.ContainsKey(bagname))
                    b = bags[bagname];
                else
                {
                    b = new Bag(bagname);
                    bags.Add(bagname, b);
                }

                if (splits[4] != "no")
                {
                    int x = 4;
                    while (x < splits.Length)
                    {
                        int nbags = Int32.Parse(splits[x]);
                        string cbagname = splits[x + 1] + " " + splits[x + 2];
                        x += 4;

                        Bag cbag;
                        if (bags.ContainsKey(cbagname))
                        {
                            cbag = bags[cbagname];
                        }
                        else
                        {
                            cbag = new Bag(cbagname);
                            bags.Add(cbagname, cbag);
                        }
                        if (cbagname == "shiny gold")
                            Console.WriteLine("shiny gold contained in " + bagname);

                        b.addContains(cbag, nbags);
                    }
                }

                //Console.WriteLine($"Bag {bagname} contains {b.contains.Count} bag types");
            }

            var shinygold = bags["shiny gold"];
            int xcount = bagsContained(shinygold) - 1;

            Console.WriteLine($"Contained in {xcount} bag types potentially.");
        }

        private static int bagTypesContainedIn(Bag shinygold, int xcount)
        {
            Stack<Bag> st = new Stack<Bag>();
            st.Push(shinygold);

            while (st.Count > 0)
            {
                var b = st.Pop();
                if (b.mark)
                    continue;

                b.mark = true;
                xcount++;

                foreach (var cb in b.containedin)
                {
                    if (!cb.mark)
                    {
                        st.Push(cb);
                    }
                }
            }

            return xcount;
        }

        private static int bagsContained(Bag shinygold)
        {
            var xcount = 0;
            foreach(var cb in shinygold.contains.Keys)
            {
                xcount += shinygold.contains[cb] * bagsContained(cb);
            }

            return xcount + 1;
        }

    }
}
