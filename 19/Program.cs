using System;
using System.IO;
using System.Collections.Generic;

namespace _19
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var f = File.OpenRead(args[0]);
            StreamReader sr = new StreamReader(f);

            Dictionary<int,string> prerules = new Dictionary<int, string>();
            SortedDictionary<int,string> bprerules = new SortedDictionary<int, string>();
            string line;

            // read til we find a blank line.
            while((line = sr.ReadLine()).Trim() != "") {
                var splits = line.Split(":");
                int n = Int32.Parse(splits[0]);
                prerules.Add(n, splits[1].Trim());
                bprerules.Add(n, splits[1].Trim());
            }

            foreach(var (n,s) in bprerules) Console.WriteLine($"{n}: {s}");
            
            Console.ReadLine();

            string rule0;
            if (!prerules.TryGetValue(0, out rule0))
                throw new Exception("no rule 0");

            Dictionary<int,Rules> rules = new Dictionary<int, Rules>();
            parserule(0, rule0, prerules, rules);

            Console.WriteLine($"we have found {rules.Count} rules.");
            foreach(var (id,rule) in rules) {
                Console.Write($"Rule {id}::: {rule.kind}");
                if (rule.kind == Rules.Kind.Char) Console.Write($"'{rule.c}'");
                else foreach(var n in rule.tomatch) Console.Write($" {n}");
                Console.WriteLine();
            }

            Console.ReadLine();

            // we've read the blank line, rest is input.
            int count = 0;
            while ((line = sr.ReadLine()) != null) {

                var (b,pos) = matchRule(0, line, 0, rules);
                if (pos < line.Length) b = false;
                Console.WriteLine($"Matched = {b} :: " + line);
                count += b ? 1 : 0;
            }
            Console.WriteLine($"Found {count} matches");
        }

        public static void parserule(int n, string s, Dictionary<int,string> prerules, Dictionary<int,Rules> rules) {
            if (s.Trim().StartsWith('"')) {
                char x = s.Trim()[1];
                Rules r = new Rules {
                    c = x,
                    num = n,
                    kind = Rules.Kind.Char
                };
                Console.WriteLine($"Add rule {n}");
                rules.Add(n, r);
            }
            else {
                var splits = s.Trim().Split('|');
                if (splits.Length == 1) {
                    var seqs = splits[0].Split(' ',StringSplitOptions.RemoveEmptyEntries);
                    List<int> mylist = new List<int>();
                    int count = 0;
                    foreach(var seq in seqs) {
                        count += 1;
                        string text = seq;
                        int p = Int32.Parse(text);
                        if (!rules.ContainsKey(p)) {
                            parserule(p, prerules[p], prerules, rules);
                        }
                        mylist.Add(p);
                    }
                    Rules r = new Rules {
                        num = n,
                        kind = Rules.Kind.Seq,
                        tomatch = mylist
                    };
                    Console.WriteLine($"Add rule {n}");
                    if (!rules.ContainsKey(n)) rules.Add(n, r);
                }
                else {
                    int count = 1;
                    List<int> toadd = new List<int>();
                    foreach(var sp in splits) {
                        int newnum = (n * 1000) + (count * n);
                        parserule(newnum, sp, prerules, rules);
                        toadd.Add(newnum);
                        count += 1;
                    }
                    if (!rules.ContainsKey(n))
                        rules.Add(n, new Rules { kind = Rules.Kind.Choice, num = n, tomatch = toadd });
                }
            }
        }

        public static (bool,int) matchRule(int r, string line, int pos, Dictionary<int,Rules> rules) {

            if (pos >= line.Length) return (false, pos);

            Rules rule = rules[r];
            bool match;
            int mypos;

            switch (rule.kind) {
                case Rules.Kind.Char:
                    //Console.WriteLine($"match {line[pos]} with {rule.c}");
                    if (line[pos] == rule.c) return (true, pos+1);
                    else return (false, pos);
                case Rules.Kind.Seq:
                    match = true;
                    mypos = pos;
                    foreach(var seq in rule.tomatch) {
                        //Console.WriteLine($"seq match {line.Substring(mypos)} to {seq}");
                        var (b, npos) = matchRule(seq, line, mypos, rules);
                        mypos = npos;
                        match = match && b;
                    }
                    if (match) return (true, mypos);
                    else return (false, pos);
                case Rules.Kind.Choice:
                    match = false;
                    mypos = pos;
                    foreach(var ch in rule.tomatch) {
                        //Console.WriteLine($"cho match {line.Substring(pos)} to {ch}");
                        var (b, npos) = matchRule(ch, line, pos, rules);
                        if (b && npos > mypos) mypos = npos;
                        match = match || b;
                    }
                    if (match) return (true, mypos);
                    else return (false, pos);
            }
            return (false,-1);
        }
    }
}
