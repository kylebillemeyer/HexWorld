using UnityEngine;
using UnityEditor;
using HexWorld.Components;

public class StateMachine
{
    public State Previous { get; set; }
    public State Current { get; set; }

    private State next;

    public void Update(GameWorld world)
    {
        if (next != null)
        {
            Current.onExit();
            Previous.onLost();

            Previous = Current;
            Current = next;
            next = null;

            Current.onEnter();
        }

        Current.Update(world);
    }
}