using UnityEngine;
using UnityEditor;
using HexWorld.Components;

namespace HexWorld.Turn
{
    public class Draw : State
    {
        public Draw(StateMachine machine) : base(machine) { }

        public override void Update(GameWorld world)
        {

        }
    }
}