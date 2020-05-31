using UnityEngine;
using System.Collections;

using HexWorld.Components;
using HexWorld.Components.Tile;

namespace HexWorld.LevelEditor
{
    public class HeightBrush : Brush
    {
        private static readonly float CAST_INTERVAL = 1000f;

        public int Dir { get; private set; }

        public HeightBrush(int dir, GameGrid grid, Camera mainCamera) : base(grid, mainCamera)
        {
            Dir = dir;
        }

        private float timeSinceLastCast = 0f;
        private bool allowCast = false;

        private Hex lastHighlighted;

        public override void Update()
        {
            var deltaMS = Time.deltaTime * 1000;

            timeSinceLastCast += deltaMS;
            if (timeSinceLastCast > CAST_INTERVAL)
            {
                timeSinceLastCast = 0;
                allowCast = true;
            }

            if (Input.GetMouseButton(0))
            {
                var hex = Grid.RayDetect(MainCamera, PhysicsLayers.Tile).GetComponent<Hex>();
                if ((hex && lastHighlighted == null) || (hex && (hex != lastHighlighted || allowCast)))
                {
                    Debug.Log(hex.Height);
                    allowCast = false;
                    timeSinceLastCast = 0f;

                    hex.UpdateHeight(hex.Height + Dir);
                    lastHighlighted = hex;
                }
            }
            else
            {
                allowCast = true;
                timeSinceLastCast = 0f;
            }
        }
    }
}
