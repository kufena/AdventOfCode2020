using System;
using System.IO;
using System.Collections.Generic;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var f = File.OpenRead(args[0]);
            var sr = new StreamReader(f);

            string line;
            bool first = true;
            Dictionary<int, HandOfCards> hands = new Dictionary<int, HandOfCards>();
            int currentplayer = 0;
            HandOfCards currenthoc;
            List<int> currenthand = new List<int>();
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("Player"))
                {
                    if (!first)
                    {
                        currenthoc = new HandOfCards(currenthand);
                        hands.Add(currentplayer, currenthoc);
                    }
                    first = false;
                    currentplayer = Int32.Parse(line.Substring(6, line.Trim().Length - 7));
                    currenthand = new List<int>();
                }
                else if (line.Trim() == "")
                {
                    ; // skip, we'll see a Player or end of line soon.
                }
                else
                {
                    // should be an int.
                    int card = Int32.Parse(line);
                    currenthand.Add(card);
                }
            }

            currenthoc = new HandOfCards(currenthand);
            hands.Add(currentplayer, currenthoc);

            // should now have players.
            Console.WriteLine($"We have {hands.Count} players.");
            foreach (var (p, h) in hands) Console.WriteLine($"  Player {p} has {h.Count} cards.");

            if (hands.Count != 2) throw new Exception("Too many hands or too few!");
            HandOfCards player1 = hands[1];
            HandOfCards player2 = hands[2];
            Part1GamePlay(player1, player2);
        }

        private static void Part1GamePlay(HandOfCards player1, HandOfCards player2)
        {
            // Playing the game.
            Console.WriteLine("Let's Play!");
            while (player1.Count > 0 && player2.Count > 0)
            {
                int p1card = player1.PopTop();
                int p2card = player2.PopTop();

                if (p1card > p2card)
                {
                    // p1 wins, gets both cards, highest first.
                    player1.AddToBottom(p1card);
                    player1.AddToBottom(p2card);
                }
                else
                {
                    // p2 wins, gets both cards, highest first.
                    player2.AddToBottom(p2card);
                    player2.AddToBottom(p1card);
                }
            }

            var winner = (player1.Count > 0) ? player1 : player2;

            int x = winner.Count;
            long sum = 0;
            while (winner.Count > 0)
            {
                int c = winner.PopTop();
                sum += c * x;
                x -= 1;
            }

            Console.WriteLine($"The final sum is {sum}");
        }
    }
}
