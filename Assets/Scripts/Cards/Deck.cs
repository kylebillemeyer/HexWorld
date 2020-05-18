using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

using HexWorld.Util;

namespace HexWorld.Cards
{
    public class Deck
    {
        public List<Card> Cards { get; set; }
        public List<Card> DrawPile { get; set; }
        public List<Card> DiscardPile { get; set; }
        public List<Card> Hand { get; set; }

        public Deck()
        {
            DiscardPile.AddRange(Cards);
            Shuffle();
        }

        public Card Draw()
        {
            if (DrawPile.Count == 0)
            {
                Shuffle();
            }

            var top = DrawPile[0];
            DrawPile.RemoveAt(0);
            Hand.Add(top);

            return top;
        }

        public void Discard(Card card)
        {
            Hand.Remove(card);
            DiscardPile.Add(card);
        }

        private void Shuffle()
        {
            DiscardPile.Shuffle();
            DrawPile.AddRange(DiscardPile);
            DiscardPile.Clear();
        }
    }
}