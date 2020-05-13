using UnityEngine;
using System.Collections;

public abstract class Brush
{
    public Grid Grid { get; private set; }
    public Camera MainCamera { get; private set; }

    public Brush(Grid grid, Camera mainCamera)
    {
        Grid = grid;
        MainCamera = mainCamera;
    }

    // Update is called once per frame
    public abstract void Update();
}
