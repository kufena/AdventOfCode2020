using System;

namespace Day25
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //long doorpkey = 17807724;
            //long cardpkey = 5764801;

            long doorpkey = 13316116;
            long cardpkey = 13651422;

            long subjectnum = 7;

            long divv = 20201227;

            long value = 1;
            long adder = 7;

            int c = 0;
            long rem;

            while (true)
            {
                value = (value * subjectnum);
                Math.DivRem(value, divv, out rem);
                //Console.WriteLine($"value={value}, divv={divv}, rem={rem}");
                value = rem;

                if (value == doorpkey)
                {
                    Console.WriteLine($"loop value is {c}");
                    break;
                }
                c += 1;
            }

            value = 1;
            subjectnum = cardpkey;

            for (int i = 0; i <= c; i++)
            {
                value = (value * subjectnum);
                Math.DivRem(value, divv, out rem);
                //Console.WriteLine($"value={value}, divv={divv}, rem={rem}");
                value = rem;
            }

            Console.WriteLine($"Final key is {value}");

        }
    }
}