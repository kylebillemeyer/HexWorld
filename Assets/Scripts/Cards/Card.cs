using HexWord.Util;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HexWorld.Cards
{
    public abstract class Card
    {
        [SerializeField]
        private string refId;
        public string RefId
        {
            get { return refId; }
            set { refId = value; }
        }

        [SerializeField]
        private string cardName;
        public string CardName
        {
            get { return cardName; }
            set { cardName = value; }
        }

        [SerializeField]
        private CardType type;
        public CardType Type
        {
            get { return type; }
            set { type = value; }
        }


        [SerializeField]
        private int cost;
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        [SerializeField]
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}