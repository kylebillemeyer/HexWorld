using HexWord.Graph;
using HexWord.Util;
using HexWorld.Components;
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
        private int powerCost;
        public int PowerCost
        {
            get { return powerCost; }
            set { powerCost = value; }
        }

        [SerializeField]
        private int focusCost;
        public int FocusCost
        {
            get { return focusCost; }
            set { focusCost = value; }
        }

        [SerializeField]
        private int soulCost;
        public int SoulCost
        {
            get { return soulCost; }
            set { soulCost = value; }
        }

        [SerializeField]
        private int spiritCost;
        public int SpiritCost
        {
            get { return spiritCost; }
            set { spiritCost = value; }
        }

        [SerializeField]
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public abstract IPattern GetPattern();
    }
}