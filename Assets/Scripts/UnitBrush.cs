using UnityEngine;
using UnityEditor;

internal class UnitBrush : Brush
{
    private GameObject unitFab;
    private Grid grid;
    private Camera main;

    public UnitBrush(GameObject unitFab, Grid grid, Camera mainCamera) : base(grid, mainCamera)
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
                var unitInst = Grid.Instantiate(unitFab);
                var unit = unitInst.GetComponent<Unit>();
                unit.Range = 3;
                Grid.PlaceUnit(unit, Grid.Tiles.Reverse[hex]);

                previousDetection = hex;
            }
        }
    }
}