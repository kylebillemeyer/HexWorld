using UnityEngine;
using UnityEditor;
using HexWorld.Components;

namespace HexWorld.Editor
{
    internal class UnitBrush : Brush
    {
        private readonly GameObject unitFab;

        public UnitBrush(GameObject unitFab, GameGrid grid, Camera mainCamera) : base(grid, mainCamera)
        {
            this.unitFab = unitFab;
        }

        private Hex previousDetection;
        public override void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var hex = Grid.RayDetectHex(MainCamera);
                if ((hex && !previousDetection) || (hex && !hex.Equals(previousDetection)))
                {
                    var unitInst = GameGrid.Instantiate(unitFab);
                    var unit = unitInst.GetComponent<Unit>();
                    unit.Range = 3;
                    Grid.PlaceUnit(unit, Grid.Tiles.Reverse[hex]);

                    previousDetection = hex;
                }
            }
        }
    }
}