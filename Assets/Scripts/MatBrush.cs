using UnityEngine;
using System.Collections;

public class MatBrush : Brush
{
    private static readonly float CAST_INTERVAL = 0f;

    public Material Material { get; private set; }
    public Grid Grid { get; private set; }
    public Camera MainCamera { get; private set; }

    public MatBrush(Material material, Grid grid, Camera mainCamera)
    {
        Material = material;
        Grid = grid;
        MainCamera = mainCamera;
    }
    
    private float timeSinceLastCast = 0f;
    private bool allowCast = true;

    public void Update()
    {
        var deltaMS = Time.deltaTime * 1000;

        timeSinceLastCast += deltaMS;
        if (timeSinceLastCast > CAST_INTERVAL)
        {
            timeSinceLastCast = 0;
            allowCast = true;
        }

        if (allowCast && Input.GetMouseButton(0))
        {
            allowCast = false;
            var hex = Grid.RayDetectHex(MainCamera);
            if (hex)
            {
                hex.OrigMaterial = Material;
                hex.UpdateMaterial(Material);
            }
        }
    }
}
