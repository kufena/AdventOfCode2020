using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var f = File.OpenRead(@"e:/work/adventofcode/21/Day21/Day21/input.txt");
            StreamReader sr = new StreamReader(f);

            string line;
            Dictionary<string, long> wordcount = new Dictionary<string, long>();
            Dictionary<string, HashSet<string>> allergies = new Dictionary<string, HashSet<string>>();

            while ((line = sr.ReadLine()) != null)
            {
                var splits = line.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                var allergysplits = splits[1].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var wordsplits = splits[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var wordhash = new HashSet<string>();
                HashSet<string> localallergies = new HashSet<string>();
                for (int i = 1; i < allergysplits.Length; i++)
                {
                    localallergies.Add(allergysplits[i]);
                }


                foreach (var word in wordsplits)
                {
                    if (wordcount.ContainsKey(word))
                        wordcount[word] += 1;
                    else
                        wordcount.Add(word, 1);
                    wordhash.Add(word);
                }

                foreach(var al in localallergies)
                {
                    if (allergies.ContainsKey(al))
                    {
                        allergies[al].IntersectWith(wordhash);
                    }
                    else
                    {
                        HashSet<string> copy = new HashSet<string>();
                        foreach (var w in wordhash) copy.Add(w);
                        allergies.Add(al, copy);
                    }
                }
            }

            long count = 0;
            foreach(var word in wordcount.Keys)
            {
                bool mentioned = false;
                foreach(var (al, contents) in allergies)
                {
                    if (contents.Contains(word))
                    {
                        mentioned = true;
                        break;
                    }
                }
                long loc = wordcount[word];
                if (!mentioned) count += loc;
                
            }

            WriteLine($"Wordcount is {count}");

            // Part2.
            SortedDictionary<string, string> finallist = new SortedDictionary<string, string>();
            while (allergies.Count > 0)
            {
                List<string> wordremoval = new List<string>();
                List<string> allergyremoval = new List<string>();
                foreach (var (k, w) in allergies)
                {
                    if (w.Count == 1)
                    {
                        allergyremoval.Add(k);
                        foreach (var t in w)
                        {
                            wordremoval.Add(t); // should only happen once.
                            finallist.Add(k, t);
                        }
                    }
                }

                foreach (var al in allergyremoval) allergies.Remove(al);
                foreach (var (k, w) in allergies) foreach (var t in wordremoval) w.Remove(t);
            }

            foreach (var (p, q) in finallist)
                Write($"{q},");
        }
    }
}
