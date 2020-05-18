using UnityEngine;
using UnityEditor;
using HexWorld.Components;
using System;

namespace HexWord.Util
{
    public abstract class State
    {
        protected StateMachine machine;

        public State(StateMachine machine)
        {
            this.machine = machine;
        }

        public abstract void Update(GameWorld world);

        public virtual void OnExit(GameWorld world)
        {
        }

        public virtual void OnLost(GameWorld world)
        {
        }

        public virtual void OnEnter(GameWorld world)
        {
        }
    }
}