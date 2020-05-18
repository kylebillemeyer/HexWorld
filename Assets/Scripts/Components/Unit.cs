using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HexWorld.Graph;

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

        public IMovementStrategy GetMovementStrategy()
        {
            return new StraightMoveStrategy(Range);
        }

        public bool IsDead()
        {
            return health == 0;
        }
    }
}
