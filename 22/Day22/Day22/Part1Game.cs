using System;
using System.Collections.Generic;
using System.Text;

namespace Day22
{
    class Part1Game
    {

        public static int Part1GamePlay(HandOfCards player1, HandOfCards player2)
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

            int winner = (player1.Count > 0) ? 1 : 2;
            var winnershand = (player1.Count > 0) ? player1 : player2;
            long sum = ProduceWinningSum(winnershand);

            Console.WriteLine($"The final sum is {sum}");

            return winner;
        }

        public static long ProduceWinningSum(HandOfCards winnershand)
        {
            long sum = 0;
            int x = winnershand.Count;
            while (winnershand.Count > 0)
            {
                int c = winnershand.PopTop();
                sum += c * x;
                x -= 1;
            }

            return sum;
        }
    }
}
