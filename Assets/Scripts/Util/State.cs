using UnityEngine;
using UnityEditor;
using HexWorld.Components;
using System;

public abstract class State
{
    protected StateMachine machine;

    public State(StateMachine machine)
    {
        this.machine = machine;
    }

    public abstract void Update(GameWorld world);

    internal void onExit()
    {
        throw new NotImplementedException();
    }

    internal void onLost()
    {
        throw new NotImplementedException();
    }

    internal void onEnter()
    {
        throw new NotImplementedException();
    }
}