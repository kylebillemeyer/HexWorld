using UnityEngine;
using System.Collections;

using HexWorld.Components;

namespace HexWorld.Editor
{
    public abstract class Brush
    {
        public GameGrid Grid { get; private set; }
        public Camera MainCamera { get; private set; }

        public Brush(GameGrid grid, Camera mainCamera)
        {
            Grid = grid;
            MainCamera = mainCamera;
        }

        // Update is called once per frame
        public abstract void Update();
    }
}