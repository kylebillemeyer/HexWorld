using UnityEngine;
using System.Collections;

using HexWorld.Components;
using HexWorld.Components.Tile;

namespace HexWorld.LevelEditor
{
    public class MatBrush : Brush
    {
        public Terrain Terrain { get; private set; }

        public MatBrush(Terrain terrain, GameGrid grid, Camera mainCamera) : base(grid, mainCamera)
        {
            Terrain = terrain;
        }

        private Hex previousDetection;
        public override void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var hex = Grid.RayDetect(MainCamera, PhysicsLayers.Tile).GetComponent<Hex>();
                if ((hex && !previousDetection) || (hex && !hex.Equals(previousDetection)))
                {
                    hex.Terrain = Terrain;
                    hex.ResetMaterial();
                    previousDetection = hex;
                }
            }
        }
    }
}
