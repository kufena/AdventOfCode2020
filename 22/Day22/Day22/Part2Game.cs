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

            HandOfCards currentPlayer1 = player1;
            HandOfCards currentPlayer2 = player2;

            while (!seenBefore(currentPlayer1,currentPlayer2, previousHands))
            {

                // First, make a not of the hands in our previous list.
                previousHands.Add((currentPlayer1.Copy(), currentPlayer2.Copy()));

                // If either hand is empty then we have a winner!
                if (currentPlayer1.Count == 0 || currentPlayer2.Count == 0)
                    break;

                // start the round - draw two cards, ensure for each player, they have at least that many
                // cards in their hands.  
                int player1Top = currentPlayer1.PopTop();
                int player2Top = currentPlayer2.PopTop();

                if (currentPlayer1.Count >= player1Top && currentPlayer2.Count >= player2Top) // we can sub game.
                {
                    // assumes different numbers of course.
                    int high = player1Top > player2Top ? player1Top : player2Top;
                    int low = high == player1Top ? player2Top : player1Top;

                    var play1subhand = new HandOfCards(currentPlayer1, player1Top);
                    var play2subhand = new HandOfCards(currentPlayer2, player2Top);

                    int subwinner = Part1Game.Part1GamePlay(play1subhand, play2subhand);

                    if (subwinner == 1)
                    {
                        currentPlayer1.AddToBottom(high);
                        currentPlayer1.AddToBottom(low);
                    }
                    else
                    {
                        currentPlayer2.AddToBottom(high);
                        currentPlayer2.AddToBottom(low);
                    }

                }
                // If not, highest card wins, and we go round again.
                else
                {
                    if (player1Top > player2Top)
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

                // go round again.
            }

            if (currentPlayer1.Count > 0 && currentPlayer2.Count > 0)
                return -1;

            int winner = currentPlayer1.Count == 0 ? 1 : 2;
            HandOfCards winnershand = currentPlayer1.Count == 0 ? currentPlayer2 : currentPlayer1;

            long finalsum = Part1Game.ProduceWinningSum(winnershand);

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
