using HexWord.Cards;
using HexWord.Graph;
using HexWord.Util;
using HexWorld.Cards;
using HexWorld.Components;
using HexWorld.Components.Tile;
using HexWorld.Graph;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexWord.Battle.States
{
    public class PatternPlacementSelect : State
    {
        private Card selectedCard;
        private Unit unit;
        private List<CubeIndex> highlightedTiles;
        private Hex hoveredTile;
        private HandUI handUI;

        public PatternPlacementSelect(Card selectedCard, Unit unit, StateMachine machine) : base(machine)
        {
            this.selectedCard = selectedCard;
            this.unit = unit;
            this.highlightedTiles = new List<CubeIndex>();
            this.handUI = GameObject.Find("HandUI").GetComponent<HandUI>();
        }

        public override void OnEnter(GameWorld world)
        {
            var index = world.Grid.PlayerUnits.Reverse[unit];
            var hex = world.Grid.Tiles.Forward[index];

            hex.UpdateMaterial(TerrainCache.Cache[Terrain.Selection]);
        }

        public override void OnExit(GameWorld world)
        {
            var index = world.Grid.PlayerUnits.Reverse[unit];
            var hex = world.Grid.Tiles.Forward[index];

            hex.ResetMaterial();
            ClearHighlightedTiles(world.Grid);
        }

        public override void Update(GameWorld world)
        {
            var hit = world.Grid.RayDetect(Camera.main, PhysicsLayers.Tile);
            if (hit != null)
            {
                var selectedIndex = world.Grid.PlayerUnits.Reverse[unit];
                var seletedTile = world.Grid.Tiles.Forward[selectedIndex];

                var hex = hit.GetComponent<Hex>();

                if (hex != seletedTile && hex != hoveredTile)
                {
                    hoveredTile = hex;

                    ClearHighlightedTiles(world.Grid);

                    var hovered_index = world.Grid.Tiles.Reverse[hoveredTile];
                    var facingNeighbor = CubeIndex.nearestIntersectingNeighbor(selectedIndex, hovered_index);
                    var facingDir = HexDirUtil.fromNeighbor(selectedIndex, facingNeighbor);
                    //highlightedTiles = new List<CubeIndex>() { selectedIndex.GetNeighbor(HexDir.Southeast) };
                    highlightedTiles = selectedCard.GetPattern().CalcTargets(selectedIndex, facingDir, world.Grid);

                    HighlightTiles(world.Grid);
                }
            }

            if (Input.GetMouseButtonDown(1) && !handUI.Busy)
            {
                handUI.CancelCardSelection();
                machine.ChangeState(new CardSelect(machine));
            }
        }

        private void ClearHighlightedTiles(GameGrid grid)
        {
            foreach (var index in highlightedTiles)
            {
                var hex = grid.Tiles.Forward[index];
                hex.ResetMaterial();
            }
        }

        private void HighlightTiles(GameGrid grid)
        {
            foreach (var index in highlightedTiles)
            {
                var hex = grid.Tiles.Forward[index];
                hex.UpdateMaterial(TerrainCache.Cache[Terrain.Pattern]);
            }
        }
    }
}