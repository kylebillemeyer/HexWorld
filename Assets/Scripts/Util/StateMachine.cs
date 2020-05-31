using UnityEngine;
using UnityEditor;
using HexWorld.Components;
using HexWord.Battle;
using System;

namespace HexWord.Util
{
    public class StateMachine
    {
        public State Previous { get; set; }
        public State Current { get; set; }

        private State next;

        public void Update(GameWorld world)
        {
            if (next != null)
            {
                if (Current != null)
                    Current.OnExit(world);
                if (Previous != null)
                    Previous.OnLost(world);

                Previous = Current;
                Current = next;
                next = null;

                Current.OnEnter(world);
            }

            Current.Update(world);
        }

        public void ChangeState(State state)
        {
            next = state;
        }

        internal void ReturnToPrevious(GameWorld world)
        {
            Current.OnExit(world);
            Current = Previous;
            Previous = null;
        }
    }
}