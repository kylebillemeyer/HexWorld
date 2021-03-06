﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HexWorld.Graph;
using HexWorld.Graph.Movement;

namespace HexWorld.Components
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private int range;
        public int Range
        {
            get { return range; }
            set { range = value; }
        }

        [SerializeField]
        private int health;
        public int Health
        {
            get { return health; }
            set { health = Math.Max(0, value); }
        }

        [SerializeField]
        private int maxHealth;
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        [SerializeField]
        private int mana;
        public int Mana
        {
            get { return mana; }
            set { mana = value; }
        }

        [SerializeField]
        private int maxMana;
        public int MaxMana
        {
            get { return maxMana; }
            set { maxMana = value; }
        }

        [SerializeField]
        private int power;
        public int Power
        {
            get { return power; }
            set { power = value; }
        }

        [SerializeField]
        private int focus;
        public int Focus
        {
            get { return focus; }
            set { focus = value; }
        }

        [SerializeField]
        private int soul;
        public int Soul
        {
            get { return soul; }
            set { soul = value; }
        }

        [SerializeField]
        private int spirit;
        public int Spirit
        {
            get { return spirit; }
            set { spirit = value; }
        }

        public IMovementStrategy GetMovementStrategy()
        {
            return new Line(Range);
        }

        public bool IsDead()
        {
            return health == 0;
        }
    }
}
