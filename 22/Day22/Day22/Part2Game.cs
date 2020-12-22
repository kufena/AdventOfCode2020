using System;
using System.Collections.Generic;
using System.Text;

namespace Day22
{
    class Part2Game
    {

        public static int Part2GamePlay(HandOfCards player1, HandOfCards player2)
        {
            List<(HandOfCards, HandOfCards)> previousHands = new List<(HandOfCards, HandOfCards)>();
            return Part2GamePlay(player1, player2, 1, previousHands);
        }

        private static int Part2GamePlay(HandOfCards player1, HandOfCards player2, int level, List<(HandOfCards, HandOfCards)> previousHands)
        {

            HandOfCards currentPlayer1 = player1;
            HandOfCards currentPlayer2 = player2;

            int gamecount = 1;

            while (!seenBefore(currentPlayer1,currentPlayer2, previousHands))
            {

                Console.WriteLine($"{level} == Game {gamecount}");
                Console.WriteLine($"    Player 1 : {currentPlayer1.ToString()}");
                Console.WriteLine($"    Player 2 : {currentPlayer2.ToString()}");

                // First, make a not of the hands in our previous list.
                previousHands.Add((currentPlayer1.Copy(), currentPlayer2.Copy()));

                // If either hand is empty then we have a winner!
                if (currentPlayer1.Count == 0 || currentPlayer2.Count == 0)
                    break;

                // start the round - draw two cards, ensure for each player, they have at least that many
                // cards in their hands.  
                int player1Top = currentPlayer1.PopTop();
                int player2Top = currentPlayer2.PopTop();

                // We are playing the sub game.
                if (currentPlayer1.Count >= player1Top && currentPlayer2.Count >= player2Top) // we can sub game.
                {
                    var play1subhand = new HandOfCards(currentPlayer1, player1Top);
                    var play2subhand = new HandOfCards(currentPlayer2, player2Top);

                    Console.WriteLine($"Going to a sub game to decide.");
                    int subwinner = Part2Game.Part2GamePlay(play1subhand, play2subhand, level + 1, new List<(HandOfCards, HandOfCards)>()); // previousHands);

                    if (subwinner == 1)
                    {
                        currentPlayer1.AddToBottom(player1Top);
                        currentPlayer1.AddToBottom(player2Top);
                    }
                    else
                    {
                        currentPlayer2.AddToBottom(player2Top);
                        currentPlayer2.AddToBottom(player1Top);
                    }

                }
                // If not, highest card wins, and we go round again.
                else
                {
                    if (player1Top > player2Top)
                    {
                        Console.WriteLine($"Player 1 wins as {player1Top} > {player2Top}");
                        currentPlayer1.AddToBottom(player1Top);
                        currentPlayer1.AddToBottom(player2Top);
                    }
                    else
                    {
                        Console.WriteLine($"Player 2 wins as {player2Top} > {player1Top}");
                        currentPlayer2.AddToBottom(player2Top);
                        currentPlayer2.AddToBottom(player1Top);
                    }
                }

                // go round again.
                gamecount += 1;
            }

            if (currentPlayer1.Count > 0 && currentPlayer2.Count > 0)
            {
                long partsum = Part1Game.ProduceWinningSum(currentPlayer1);
                return 1; // throw new Exception("We are out of the loop but both have non-zero counts!") ;
            }

            int winner = currentPlayer1.Count == 0 ? 2 : 1;
            HandOfCards winnershand = currentPlayer1.Count == 0 ? currentPlayer2 : currentPlayer1;

            long finalsum = Part1Game.ProduceWinningSum(winnershand);

            Console.WriteLine($"Part2 game {level} final sum is {finalsum}");

            return winner;
        }

        public static bool seenBefore(HandOfCards player1, HandOfCards player2, List<(HandOfCards,HandOfCards)> previous)
        {
            foreach(var prev in previous)
            {
                var p1prev = prev.Item1;
                var p2prev = prev.Item2;
                if (player1.ExtEquals(p1prev) && player2.ExtEquals(p2prev))
                {
                    Console.WriteLine("We have seen a hand for the second time.  We'll stop, I guess!");
                    return true;
                }
            }
            return false;
        }

    }
}
