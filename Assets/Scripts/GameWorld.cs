using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

public class GameWorld : MonoBehaviour
{
    public bool Disabled { get; set; }
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
        if (Disabled)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ResetPreviousSelections();
            selection = Grid.RayDetectHex(main_camera);
                        
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
    }

    private void ResetPossibleDestinations()
    {
        if (potentialDestinations != null)
        {
            potentialDestinations.ForEach((d) => d.UpdateMaterial(d.OrigMaterial));
            potentialDestinations = null;
        }
    }

    private void HandleNewSelection()
    {
        if (potentialDestinations != null && potentialDestinations.Contains(selection))
        {
            var src = Grid.Tiles.Reverse[previousSelection];
            var dest = Grid.Tiles.Reverse[selection];
            Grid.MoveUnit(src, dest);
            ResetPossibleDestinations();
        }
        else if (potentialDestinations != null)
        {
            ResetPossibleDestinations();
        }
        else
        {
            selection.UpdateMaterial(selected_mat);

            if (selection.Unit != null)
            {
                var selectedIndex = Grid.Tiles.Reverse[selection];

                var moveStrat = selection.Unit.GetMovementStrategy();
                this.potentialDestinations = moveStrat.CalcDestinations(selectedIndex, Grid);

                foreach (var tile in this.potentialDestinations)
                {
                    tile.UpdateMaterial(destination_mat);
                }
            }
        }
    }
}
