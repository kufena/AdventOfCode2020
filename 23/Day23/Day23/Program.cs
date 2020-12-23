using System;
using System.Collections.Generic;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string text = args[0].Trim();
            Console.WriteLine($"The text is {text}");

            int goes = Int32.Parse(args[1]);

            Link first = null;
            Dictionary<int, Link> lookup = new Dictionary<int, Link>();

            // parse string, make links, build ring, make lookup.
            Link current = null;
            int counter = 0;
            int max = -1;
            for (int i = 0; i < text.Length; i++)
            {
                int x = Int32.Parse($"{text[i]}");
                if (x > max) max = x;
                Link k = new Link
                {
                    num = x
                };
                lookup.Add(x, k);

                if (first == null)
                {
                    first = k;
                }
                if (current != null)
                {
                    current.next = k;
                }
                current = k;
                counter++;
            }

            // for part 2 we make the number of cups up to 1000000.
            int next = max + 1;
            while(counter < 1000000)
            {
                int x = next;
                if (x > max) max = x;
                Link k = new Link
                {
                    num = x
                };
                lookup.Add(x, k);
                current.next = k;
                current = k;
                next += 1;
                counter += 1;
            }

            current.next = first;


            // Count the cups, for sanity.
            Link tx = first.next;
            counter = 1;
            while (tx != first)
            {
                counter++;
                tx = tx.next;
            }

            // do our 10000000 goes.
            Console.WriteLine($"We have {counter} links we think.");
            Console.WriteLine("Let's go!");
            
            current = Part1(goes, first, lookup, max);


            // Use the look up table to find 1 and get the numbers for the sum.
            // Ie the two numbers immediately clockwise from 1.

            tx = lookup[1];
            tx = tx.next;
            long p = (long)tx.num * (long)tx.next.num;

            Console.WriteLine("Next number is 1, so we want the numbers...");
            Console.WriteLine($"   {tx.num}");
            Console.WriteLine($"   {tx.next.num}");
            Console.WriteLine($"The sume we want is {p}");

            /*

            tx = first.next;
            while (tx != first)
            {
                if (tx.next.num == 1)
                {
                    Console.WriteLine("Next number is 1, so we want the numbers...");
                    Console.WriteLine($"   {tx.num}");
                    Console.WriteLine($"   {tx.next.next.num}");
                    Console.WriteLine($"or {tx.next.next.next.num}");

                    long p = (long)tx.num * (long)tx.next.next.num;
                    long q = (long)tx.next.next.num * (long)tx.next.next.next.num;
                    Console.WriteLine($"Our sum value is {p} but it could be {q}");
                }

                tx = tx.next;
            }
            */
        }

        // This is the shuffle game with the numbers given.
        private static Link Part1(int goes, Link first, Dictionary<int, Link> lookup, int max)
        {
            Link current = first;
            for (int i = 0; i < goes; i++)
            {
                Link removed = current.next;
                current.next = removed.next.next.next;

                var p = findLink(current.num - 1, current, removed, lookup, max);
                var k = p.next;
                removed.next.next.next = p.next;
                p.next = removed;
                //Console.WriteLine($"{i} :: {makelist(current)}");

                current = current.next;
            }

            return current;
        }

        public static Link findLink(int n, Link current, Link removed, Dictionary<int,Link> lookup, int max)
        {
            while(true)
            {
                if (lookup.ContainsKey(n) && !insection(n, removed))
                    return lookup[n];

                if (!lookup.ContainsKey(n))
                {
                    n = n - 1;
                    if (n < 0) n = max;
                    continue;
                }

                if (insection(n, removed))
                {
                    n = n - 1;
                    if (n < 0) n = max;
                    continue;
                }
            }
        }

        public static bool insection(int n, Link removed)
        {
            return (
                removed.num == n ||
                removed.next.num == n ||
                removed.next.next.num == n
                );
        }
        public static string makelist(Link f)
        {
            string l = $"{f.num}";
            var lk = f.next;
            while(lk != f)
            {
                l += $"{lk.num}";
                lk = lk.next;
            }
            return l;
        }
    }
}
