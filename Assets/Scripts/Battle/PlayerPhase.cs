using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HexWorld.Components;
using System.Linq;
using HexWord.Util;

namespace HexWorld.Battle
{
    public class PlayerPhase : State
    {
        public static readonly int HAND_SIZE = 6;

        public HashSet<Unit> UnexhaustedUnits { get; set; }

        public PlayerPhase(StateMachine machine) : base(machine) { }

        public override void OnEnter(GameWorld world)
        {
            world.Deck.DrawN(6);
            UnexhaustedUnits = new HashSet<Unit>(world.Grid.Units.Forward.Values);
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
