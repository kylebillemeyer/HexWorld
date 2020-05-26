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
        public static readonly int MIN_CARD_THRESHOLD = 6;

        public List<Card> Cards { get; set; }
        public List<Card> DrawPile { get; set; }
        public List<Card> DiscardPile { get; set; }
        public List<Card> Hand { get; set; }

        public int CardCount { get { return Cards.Count - Hand.Count; } }

        public Deck()
        {
            Cards = new List<Card>();
            DrawPile = new List<Card>();
            DiscardPile = new List<Card>();
            Hand = new List<Card>();
        }

        public void Add(Card card)
        {
            Cards.Add(card);
        }

        public void AddRange(IEnumerable<Card> cards)
        {
            Cards.AddRange(cards);
        }

        public void Remove(Card card)
        {
            if (CardCount <= 0)
            {
                throw new ArgumentException("Removing card would drop deck below min threshold.");
            }

            Cards.Remove(card);
        }

        public void Reset()
        {
            DrawPile.Clear();
            DiscardPile.Clear();
            Hand.Clear();

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

        public List<Card> Draw(int n)
        {
            if (n <= CardCount)
            {
                var cards = new List<Card>();

                foreach (int i in Enumerable.Range(0, n))
                {
                    cards.Add(Draw());
                }

                return cards;
            }
            else
            {
                throw new ArgumentException("Not enough cards in deck.");
            }
        }

        public void Discard(Card card)
        {
            Hand.Remove(card);
            DiscardPile.Add(card);
        }

        public void DiscardHand()
        {
            DiscardPile.AddRange(Hand);
            Hand.Clear();
        }

        public void Shuffle()
        {
            DiscardPile.Shuffle();
            DrawPile.AddRange(DiscardPile);
            DiscardPile.Clear();
        }
    }
}