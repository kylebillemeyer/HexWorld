using UnityEngine;
using UnityEditor;
using HexWorld.Components;
using HexWord.Util;

namespace HexWorld.Battle
{
    public class Draw : State
    {
        public static readonly int HAND_SIZE = 6;

        public Draw(StateMachine machine) : base(machine) { }

        public override void OnEnter(GameWorld world)
        {
            world.Deck.DrawN(6);
        }

        public override void Update(GameWorld world)
        {

        }

        public override void OnExit(GameWorld world)
        {
            world.Deck.DiscardHand();
        }
    }
}