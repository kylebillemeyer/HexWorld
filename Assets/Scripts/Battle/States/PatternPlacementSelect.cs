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

        public PatternPlacementSelect(Card selectedCard, Unit unit, StateMachine machine) : base(machine)
        {
            this.selectedCard = selectedCard;
            this.unit = unit;
            this.highlightedTiles = new List<CubeIndex>();
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

                    var index = world.Grid.Tiles.Reverse[hoveredTile];
                    var facing = HexDir.Northeast;
                    highlightedTiles = selectedCard.GetPattern().CalcTargets(selectedIndex, facing, world.Grid);

                    HighlightTiles(world.Grid);
                }
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
                hex.UpdateMaterial(TerrainCache.Cache[Terrain.Selection]);
            }
        }
    }
}