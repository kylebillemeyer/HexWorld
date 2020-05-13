using UnityEngine;
using System.Collections;

public class MatBrush : Brush
{
    private static readonly float CAST_INTERVAL = 0f;

    public Material Material { get; private set; }

    public MatBrush(Material material, Grid grid, Camera mainCamera) : base(grid, mainCamera)
    {
        Material = material;
    }

    private Hex previousDetection;
    public override void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var hex = Grid.RayDetectHex(MainCamera);
            if ((hex && !previousDetection) || (hex && !hex.Equals(previousDetection)))
            {
                hex.OrigMaterial = Material;
                hex.UpdateMaterial(Material);
                previousDetection = hex;
            }
        }
    }
}
