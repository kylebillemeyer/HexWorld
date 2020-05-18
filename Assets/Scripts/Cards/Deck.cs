using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

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

        public Card DrawCard()
        {
            if (DrawPile.Count == 0)
            {
                Shuffle();
            }

            var top = DrawPile[0];
            DrawPile.RemoveAt(0);

            return top;
        }

        private void Shuffle()
        {
            //DiscardPile.Shuff
        }
    }
}