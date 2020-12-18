using System;
using System.IO;
using System.Collections.Generic;

namespace _18
{

    public class Parser
    {
        public static long Parse(string line) {
            var tokens = Tokenize(line);
            Console.WriteLine("TOKENSEOKNTSTOKENS::::::");
            foreach(var s in tokens) Console.WriteLine(s);
            Console.WriteLine("TOKENSEOKNTSTOKENS::::::");
            Console.WriteLine();
            var (result, ind) = ConsumeTokens(0, tokens);
            return result;
        }

        public static long ParsePart2(string line) {
            var tokens = Tokenize(line);
            Console.WriteLine("TOKENSEOKNTSTOKENS::::::");
            foreach(var s in tokens) Console.WriteLine(s);
            Console.WriteLine("TOKENSEOKNTSTOKENS::::::");
            Console.WriteLine();
            var (result, ind) = ConsumeTokensPart2(0, tokens);
            return result;
        }

        public static (long, int) ConsumeTokensPart2(int index, List<string> tokens) {
            
            long result = 0;
            bool op = false;
            string opstr = "";
            Stack<long> stack = new Stack<long>();

            while (index < tokens.Count) {
                if (tokens[index].StartsWith("num::")) {
                    long x = Int64.Parse(tokens[index].Substring(5));
                    Console.WriteLine($"Found the number {x}");
                    if (op && opstr == "plus")                     {
                        Console.WriteLine($"Multiplying {result} by {x} to get {result + x}");
                        result = result + x;
                    }
                    if (!op) {
                        Console.WriteLine($"No op yet so result become {x}");
                        result = x;
                    }
                    op = false;
                }
                else if (tokens[index] == "(") {
                    Console.WriteLine("Open bracket - recursing.");
                    var (x, newindex) = ConsumeTokensPart2(index+1, tokens);
                    if (op && opstr == "plus")                     {
                        Console.WriteLine($"Multiplying {result} by {x} to get {result + x}");
                        result = result + x;
                    }
                    if (!op) {
                        Console.WriteLine($"No op yet so result become {x}");
                        result = x;
                    }
                    op = false;
                    index = newindex;
                }
                else if (tokens[index] == ")") {
                    stack.Push(result);
                    result = 1;
                    foreach(var l in stack) result = result * l;
                    Console.WriteLine($"Closed bracket so returning {result}");
                    return (result, index);
                }
                else if (tokens[index] == "plus") {
                    Console.WriteLine("We found the operator " + tokens[index]);
                    op = true;
                    opstr = tokens[index];
                } if (tokens[index] == "mult") {
                    stack.Push(result);
                    result = 0;
                    op = false;
                    opstr = "";
                }
                index += 1;
            }

            stack.Push(result);
            result = 1;
            foreach(var l in stack) {
                Console.WriteLine($"mult {result} by {l} sub sum is {result * l}");
                result = result * l;
            }
            return (result, index);
        }

        public static (long, int) ConsumeTokens(int index, List<string> tokens) {
            
            long result = 0;
            bool op = false;
            string opstr = "";
            
            while (index < tokens.Count) {
                if (tokens[index].StartsWith("num::")) {
                    long x = Int64.Parse(tokens[index].Substring(5));
                    Console.WriteLine($"Found the number {x}");
                    if (op && opstr == "mult")
                    {
                        Console.WriteLine($"Multiplying {result} by {x} to get {result * x}");
                         result = result * x;
                    }
                    if (op && opstr == "plus")                     {
                        Console.WriteLine($"Multiplying {result} by {x} to get {result + x}");
                        result = result + x;
                    }
                    if (!op) {
                        Console.WriteLine($"No op yet so result become {x}");
                        result = x;
                    }
                    op = false;
                }
                else if (tokens[index] == "(") {
                    Console.WriteLine("Open bracket - recursing.");
                    var (x, newindex) = ConsumeTokens(index+1, tokens);
                    if (op && opstr == "mult")
                    {
                        Console.WriteLine($"Multiplying {result} by {x} to get {result * x}");
                         result = result * x;
                    }
                    if (op && opstr == "plus")                     {
                        Console.WriteLine($"Multiplying {result} by {x} to get {result + x}");
                        result = result + x;
                    }
                    if (!op) {
                        Console.WriteLine($"No op yet so result become {x}");
                        result = x;
                    }
                    op = false;
                    index = newindex;
                }
                else if (tokens[index] == ")") {
                    Console.WriteLine($"Closed bracket so returning {result}");
                    return (result, index);
                }
                else if (tokens[index] == "mult" || tokens[index] == "plus") {
                    Console.WriteLine("We found the operator " + tokens[index]);
                    op = true;
                    opstr = tokens[index];
                }
                index += 1;
            }

            return (result, index);
        }

        public static List<string> Tokenize(string line) {
            List<string> result = new List<string>();
            var splits = line.Split(' ');

            foreach(var split in splits) {
                Console.WriteLine($"TOKENIZING {split}");
                bool innum = false;
                long acc = 0;
                for(int i = 0; i < split.Length; i++) {
                    if (Char.IsDigit(split[i])) {
                        innum = true;
                        acc = acc * 10;
                        acc += ToDigit(split[i]);
                    }
                    else {
                        if (innum) {
                            result.Add("num::" + acc);
                            acc = 0;
                            innum = false;
                        }

                        if (split[i] == '*')
                            result.Add("mult");
                        else if (split[i] == '+')
                            result.Add("plus");
                        else if (split[i] == '(')
                            result.Add("(");
                        else if (split[i] == ')')
                            result.Add(")");
                        else
                            throw new Exception ($"Illegal in {split}");
                    }
            }
                if (innum)
                    result.Add("num::" + acc);
            }

            return result;
        }

        static int ToDigit(char c) {
            if (c == '0') return 0;
            if (c == '1') return 1;
            if (c == '2') return 2;
            if (c == '3') return 3;
            if (c == '4') return 4;
            if (c == '5') return 5;
            if (c == '6') return 6;
            if (c == '7') return 7;
            if (c == '8') return 8;
            if (c == '9') return 9;
            throw new Exception($"Not a digit -- {c}");
        }

    }

}