using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Collections;

namespace _13
{
    class Program
    {
        public static bool done = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            part2(args);
        }

        public static void part2b(string[] args) {
            var f = File.OpenRead(args[0]);
            var sr = new StreamReader(f);
            long timet = Int64.Parse(sr.ReadLine());
            string busses = sr.ReadLine();
            Console.WriteLine(busses);

            var splits = busses.Split(',');
            int counter = 0;
            foreach(var s in splits) {
                if (s != "x")
                    counter++;
            }

            int[] a = new int[counter];
            int[] n = new int[counter];
            int c2 = 0;
            int c3 = 0;
            long N = 1;
            foreach(var s in splits)
            {
                if (s != "x")
                {
                    int b = Int32.Parse(s);
                    N = N * b;
                    a[c3] = c2;
                    n[c3] = b;
                    c3++;
                }
                c2++;
            } 

            for(int i = 0; i < counter; i++) {
                for(int j = i+1; j < counter; j++) {
                    if (n[j] > n[i]) {
                        int k = n[i];
                        int l = a[i];
                        n[i] = n[j];
                        a[i] = a[j];
                        n[j] = k;
                        a[j] = l;
                    }
                }
            }

            for (int i = 0; i < counter; i++) {
                Console.WriteLine($"{i} : {n[i]} at {a[i]}");
            }

            long mult = n[0];
            long tbase = n[0] - a[0];
            IEnumerator<long> initList = new LazyList(tbase, mult, N);

            for(int i = 1; i < counter; i++) {
                IEnumerator<long> nbase = new LazySieve(initList, n[i], a[i]);
                initList = nbase;
            }            

            if (initList.MoveNext())
                Console.WriteLine($"the answer is {initList.Current}");
        }

        public class LazySieve : IEnumerator<long>
        {

            IEnumerator<long> tbase;
            long n;
            long a;

            long current = 0;

            public LazySieve(IEnumerator<long> initList, long n, long a) {
                tbase = initList;
                this.n = n;
                this.a = a;
            }

            public long Current => current;

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                while (tbase.MoveNext()) {
                    if ((tbase.Current + a) % n == 0)
                    {
                        current = tbase.Current;
                        return true;
                    }
                }
                return false;
            }

            public void Reset()
            {
                tbase.Reset();
            }
        }

        public class LazyList : IEnumerator<long>
        {

            long s;
            long a;
            long N;

            long current;

            public LazyList(long start, long addition, long stop) {
                s = start;
                a = addition;
                current = s;
                N = stop;
            }

            public long Current => current;

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                current += a;
                return current <= N;
            }

            public void Reset()
            {
                current = s;
            }
        }


        public static void part2(string[] args) {
            var f = File.OpenRead(args[0]);
            var sr = new StreamReader(f);
            long timet = Int64.Parse(sr.ReadLine());
            string busses = sr.ReadLine();
            Console.WriteLine(busses);

            var splits = busses.Split(',');
            Dictionary<int,int> dict = new Dictionary<int, int>();
            int counter = 0;
            int largest = -1;
            int largestc = 0;

            foreach(var s in splits) {
                if (s != "x")
                {
                    int b = Int32.Parse(s);
                    dict.Add(counter, b);
                    if (b > largest) 
                    {
                        largest = b;
                        largestc = counter;
                    }
                }
                counter++;
            }

            Console.WriteLine($"We'll pivot about {largest} with index {largestc}");
            Dictionary<int,int> pivots = new Dictionary<int, int>();
            foreach(var (a,b) in dict) {
                if (b != largest) {
                    pivots.Add(b, a - largestc);
                    Console.WriteLine($"Adding bus {b} with time pivot {a - largestc}");
                }
            }

            
            int numthreads = 8;
            long[] indexes = new long[numthreads];
            long c = largestc;
            for(int i = 1; i <= numthreads; i++)
                indexes[i-1] = i * largest;
            long step = largest * numthreads;
            ThreadPool.SetMinThreads(numthreads+1,numthreads+1);

            for(int i = 0; i < numthreads; i++) {
                ThreadPool.QueueUserWorkItem(buildCallBacklong(step, indexes, i, pivots, largestc));
            }

            Thread.Sleep(Int32.MaxValue - 100);

        }

        public static WaitCallback buildCallBacklong (long step, long[] indexes, int index, Dictionary<int,int> pivots, long largestc) {
            return (x => Program.Part2Thread(step, indexes, index, pivots, largestc));
        }

        public static void Part2Thread(long step, long[] indexes, int index, Dictionary<int,int> pivots, long largestc) 
        {
            long time = indexes[index];
            int dd = 0;
            bool solved = false;

            while(!done) 
            {
                bool condition = true;
                //Console.Write(time);
                foreach(var (bus, t) in pivots) 
                {
                    long rem = (time + t) % bus;
                    //Console.Write($" {bus}:{rem}");
                    if (rem > 0) {
                        condition = false;
                        break;
                    }
                }
                //Console.WriteLine($" {condition}");
                if (condition) {
                    solved = true;
                    break;
                }
                if (dd % 1000000000 == 0)
                    Console.WriteLine(time);
                dd++;
                time += step;
            }

            done = true;

            if (solved) {
            Console.WriteLine($"AND THE ANSWER IS {time - largestc}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time} ");
            Console.WriteLine($"AND THE ANSWER IS {time - largestc}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time} ");
            Console.WriteLine($"AND THE ANSWER IS {time - largestc}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time} ");
            Console.WriteLine($"AND THE ANSWER IS {time - largestc}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time} ");
            Console.WriteLine($"AND THE ANSWER IS {time - largestc}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time} ");
            Console.WriteLine($"AND THE ANSWER IS {time - largestc}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time}  ANSWER IS {time} ");
            }
            Console.WriteLine($"Thread {index} done!");
        }

        public static void part1(string[] args) {
            var f = File.OpenRead(args[0]);
            var sr = new StreamReader(f);
            long timet = Int64.Parse(sr.ReadLine());

            Console.WriteLine($"We arrive at the bus stop at time {timet}");
            string busses = sr.ReadLine();
            Console.WriteLine(busses);

            var splits = busses.Split(',');
            long bus = -1;
            long timeb = Int64.MaxValue;

            foreach(var x in splits) {
                if (x == "x")
                    continue;

                long t = Int64.Parse(x);
                long rem;
                long period = ((Math.DivRem(timet, t, out rem)) + 1) * t;

                if (rem == 0) // exact division.
                {
                    period = 0;
                }

                Console.WriteLine($"bus {t} gets here at {period} -- {timeb} -- {period - timet}");
                if (period - timet < timeb)
                {
                    bus = t;
                    timeb = period - timet;
                }
            }

            Console.WriteLine($"We get there at {timet}, the best bus is {bus} at {timeb}");
            Console.WriteLine($"The answer is {bus * timeb}");
        }
    }
}
