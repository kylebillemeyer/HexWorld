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

        public IMovementStrategy GetMovementStrategy()
        {
            return new StraightMoveStrategy(Range);
        }
    }
}
