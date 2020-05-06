using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

public class GameWorld : MonoBehaviour
{
    public Grid Grid { get; set; }

    private Camera main_camera;
    private Material selected_mat;
    private Material destination_mat;

    private Hex selection;
    private Hex previousSelection;
    private List<Hex> potentialDestinations;

    // Use this for initialization
    void Start()
    {
        main_camera = Camera.main;
        selected_mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Selected.mat");
        destination_mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Destination.mat");

        var unitFab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/unit.prefab");

        Grid = GetComponentInChildren<Grid>();

        var unitInst = (GameObject)Instantiate(unitFab);
        var unit = unitInst.GetComponent<Unit>();
        unit.Range = 3;
        Grid.PlaceUnit(unit, new CubeIndex(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ResetPreviousSelections();
            selection = DetermineNewSelection();
                        
            if (selection != null)
            {
                HandleNewSelection();
            }
        }
    }

    private void ResetPreviousSelections()
    {
        if (selection != null)
        {
            selection.UpdateMaterial(selection.OrigMaterial);
            previousSelection = selection;
        }

        if (potentialDestinations != null)
        {
            potentialDestinations.ForEach((d) => d.UpdateMaterial(d.OrigMaterial));
        }
    }

    private Hex DetermineNewSelection()
    {
        int layerMask = 1 << 8;
        Ray ray = main_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(ray.origin, hit.point, Color.yellow);
            Debug.Log("Did Hit");

            return hit.collider.gameObject.GetComponent<Hex>();
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.white);
            Debug.Log("Did not Hit");

            return null;
        }
    }

    private void HandleNewSelection()
    {
        if (potentialDestinations.Contains(selection))
        {
            var src = Grid.Tiles.Reverse[selection];
            var dest = Grid.Tiles.Reverse[selection];
            Grid.MoveUnit(src, dest);
        }
        else
        {
            selection.UpdateMaterial(selected_mat);

            if (selection.Unit != null)
            {
                var selectedIndex = Grid.Tiles.Reverse[selection];

                var moveStrat = selection.Unit.GetMovementStrategy();
                var tiles = moveStrat.CalcDestinations(selectedIndex, Grid);

                foreach (var tile in tiles)
                {
                    tile.UpdateMaterial(destination_mat);
                }
            }
        }
    }
}
