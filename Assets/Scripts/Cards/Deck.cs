using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace HexWorld.Cards
{
    public class Deck
    {
        public List<Card> Cards { get; set; }
        public List<Card> DrawPile { get; set; }
        public List<Card> DiscardPile { get; set; }

        public Card DrawCard()
        {
            DrawPile.
        }

        private int topCard = 0;
    }
}