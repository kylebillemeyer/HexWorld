using UnityEngine;
using UnityEditor;
using HexWord.Util;
using HexWorld.Components;

public class CardSelect : State
{
    public CardSelect( StateMachine machine) : base(machine)
    {
    }

    public override void Update(GameWorld world)
    {
        // All the logic actually happens in the HandUI script. This is just an idle state.
    }
}