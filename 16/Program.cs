using System;
using System.IO;
using System.Collections.Generic;

using static System.Console;

namespace _16
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 16 Let's go!");
            var f = File.OpenRead(args[0]);
            StreamReader sr = new StreamReader(f);

            Part2(sr);

        }

        static void Part2(StreamReader streamReader) {
            bool test = false;
            long result = 0;

            string line = streamReader.ReadLine();

            if (line.StartsWith("result:")) {
                WriteLine("Stand down - it's a test!");
                test = true;
                result = Int64.Parse(line.Substring(8).Trim()); // accounts for space after :
                line= streamReader.ReadLine();
            }

            List<Rule> rules = new List<Rule>();
            List<List<int>> nearbytickets = new List<List<int>>();
            var yourticket = readFile(line, streamReader, rules, nearbytickets);
            
            List<List<int>> validnearbytickets = new List<List<int>>();
            foreach(var ticket in nearbytickets) {
                bool allok = true;
                foreach(var val in ticket) {
                    allok = allok && matchAllConstraints(rules, val);
                }
                if (allok)
                    validnearbytickets.Add(ticket);
            }

            // ok, we have our valid nearby tickets, so for each field, we collect the rules
            // that work for that field, whittling them down.
            int numfields = validnearbytickets[0].Count;

            HashSet<Rule>[] rulesperposition = new HashSet<Rule>[numfields];
            for(int i = 0; i < numfields; i++) {
                rulesperposition[i] = new HashSet<Rule>();
                foreach(var rule in rules) rulesperposition[i].Add(rule);
            }

            // this is going to be a memory carnage, but hey ho!
            int counter = 0;
            foreach(var ticket in validnearbytickets) {
                for(int i = 0; i < numfields; i++) {
                    HashSet<Rule> remainingrules = new HashSet<Rule>();
                    foreach(var rule in rules) {
                        if (matchConstraints(rule, ticket[i]))
                            remainingrules.Add(rule);
                    }
                    rulesperposition[i].IntersectWith( remainingrules );
                }
                //WriteLine($"After {counter} goes we have {rulesperposition[i].Count} rules remaining.");
                //if (rulesperposition[i].Count == 0) WriteLine($"Our current value is {ticket[i]}");
                counter++;
            }

            // let's get jiggy with the remaining rules, ruling out those that are already gone.
            // This is resolving the rules.  Any position that has only one rule must be just that
            // rule.  We remove that rule from any other position.  We repeat this until all positions
            // only have a single rule, or in other words, we stop making changes.
            bool done = false;
            while(!done) {

                bool changes = false;
                for(int i = 0; i < numfields; i++) {
                    if (rulesperposition[i].Count == 1) {
                        Rule? ourrule = null;
                        foreach(var r in rulesperposition[i]) ourrule = r;
                        if (ourrule != null) {
                            for(int j = 0; j < numfields; j++) {
                                if (i != j && rulesperposition[j].Contains(ourrule)) {
                                    changes = true;
                                    rulesperposition[j].Remove(ourrule);
                                }
                            }
                        }
                    }
                }
                done = !changes;
            }

            // makesure there's one rule for each position.
            for(int i = 0; i < numfields; i++) {
                if (rulesperposition[i].Count != 1) {
                    WriteLine($"Position {i} has {rulesperposition[i].Count} rules left.");
                }
                else {
                    Rule x = null;
                    foreach(var rule in rulesperposition[i]) x = rule;
                    // if we claim this rule, then no one else can have it!
                    for(int j = i+1; j < numfields; j++)
                        rulesperposition[j].Remove(x);
                    WriteLine($"Position {i} is rule {x.name}");
                }
            }

            // find the 'departure' rule positions.
            List<int> positions = new List<int>();
            for(int i = 0; i < numfields; i++) {
                Rule x = null;
                foreach(var rule in rulesperposition[i]) x = rule;
                if (x != null && x.name.StartsWith("departure"))
                    positions.Add(i);
            }

            long sum = 1;
            foreach(var i in positions) sum *= yourticket[i];

            WriteLine($"Sum of the departure fields is {sum}");
            if (test) {
                WriteLine($"We expected to see {result}");
            }
        }

        static void Part1(StreamReader streamReader) {
            bool test = false;
            long result = 0;

            string line = streamReader.ReadLine();

            if (line.StartsWith("result:")) {
                WriteLine("Stand down - it's a test!");
                test = true;
                result = Int64.Parse(line.Substring(8).Trim()); // accounts for space after :
                line= streamReader.ReadLine();
            }

            List<Rule> rules = new List<Rule>();
            List<List<int>> nearbytickets = new List<List<int>>();
            var yourticket = readFile(line, streamReader, rules, nearbytickets);

            long sum = 0;
            foreach(var ticket in nearbytickets) {
                foreach(var val in ticket) {
                    sum += (long) (matchAllConstraints(rules, val) ? 0 : val);
                }
            }

            WriteLine($"The sum of mismatched ticket values is {sum} ");
            if (test) {
                WriteLine($"Expected result was {result}");
            }
        }

        // match a set of rules against a value.
        static bool matchAllConstraints(List<Rule> rules, int val) {
            // again will be true if at least one rule matches.
            bool matchone = false;
            foreach(var rule in rules) {
                matchone = matchone || matchConstraints(rule, val);
            }
            return matchone;
        }

        // match a rule against a value, return whether matched or not.
        static bool matchConstraints(Rule rule, int val) {
            // matchone here will be false all the way through if no constraints match.
            bool matchone = false;
            foreach(var (lower, higher) in rule.constraints) {
                matchone = matchone || (lower <= val && val <= higher);
            }
            return matchone;
        }

        // Handles reading the file.
        static List<int> readFile(string line, StreamReader streamReader, List<Rule> rules, List<List<int>> nearbytickets) {            

            List<int> yourticket = new List<int>();

            bool brulessection = true;
            bool byourticket = false;
            bool bnearbytickets = false;

            do {

                if (line.Trim() == "")
                    continue;

                if (line.StartsWith("your ticket:")) {
                    brulessection = false;
                    byourticket = true;
                    continue;
                }

                if (line.StartsWith("nearby tickets:")) {
                    byourticket = false;
                    bnearbytickets = true;
                    continue;
                }

                if (brulessection) {
                    var colonsplit = line.Split(':');
                    List<(int,int)> constraints = new List<(int, int)>();
                    var orsplit = colonsplit[1].Split("or");
                    foreach(var constraint in orsplit) {
                        //WriteLine(constraint);
                        var commasplits = constraint.Split('-');
                        int lower = Int32.Parse(commasplits[0]);
                        int higher = Int32.Parse(commasplits[1]);
                        constraints.Add((lower,higher));
                    }
                    var Rule = new Rule { name = colonsplit[0].Trim(), constraints = constraints };
                    rules.Add(Rule);
                }

                if (byourticket) {
                    yourticket = parseTicket(line);
                }

                if (bnearbytickets) {
                    var ticket = parseTicket(line);
                    nearbytickets.Add(ticket);
                }

            } while ((line = streamReader.ReadLine()) != null);

            return yourticket;
        }

        // parses a ticket - a comma separated list of, we presume, ints.
        static List<int> parseTicket(string line) {
            List<int> values = new List<int>();
            var valsplits = line.Split(',');
            foreach(var valstr in valsplits) {
                values.Add(Int32.Parse(valstr));
            }
            return values;
        }
    }
}
