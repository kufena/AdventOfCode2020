using System;
using System.Collections.Generic;
using System.Text;

namespace Day22
{
    public class HandOfCards
    {
        List<int> cards;

        public HandOfCards(int[] cards)
        {
            this.cards = new List<int>(cards);
        }

        public HandOfCards(List<int> hand)
        {
            cards = new List<int>();
            foreach (var c in hand) cards.Add(c);
        }

        public HandOfCards(List<int> hand, int num)
        {
            cards = new List<int>();
            for (int i = 0; i < num; i++) cards.Add(hand[i]); // copy n numbers.
        }


        public HandOfCards(HandOfCards hand, int num) : this(hand.cards, num)
        { }

        public HandOfCards Copy()
        {
            return new HandOfCards(cards);
        }

        public int Count { get => cards.Count; }

        public int PopTop()
        {
            if (cards.Count == 0)
                return -1;

            int result = cards[0];
            cards.Remove(result);
            return result;
        }

        public void AddToBottom(int c)
        {
            cards.Add(c);
        }

        // Let's make this clear it's an extensional equality on the order and
        // contents of the cards, and not the Equals provided by any sub/base class.
        public bool ExtEquals(HandOfCards hoc)
        {
            if (hoc.cards.Count == cards.Count)
            {
                for(int i = 0; i < cards.Count; i++)
                {
                    if ((hoc.cards[i]) != (cards[i]))
                        return false;
                }
                return true;
            }
            else
                return false;
        }
    }
}
