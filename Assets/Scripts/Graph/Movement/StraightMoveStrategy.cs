using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using HexWorld.Components;
using HexWorld.Components.Tile;
using HexWorld.Graph;
using HexWord.Graph;

namespace HexWorld.Graph.Movement
{
    public class StraightMoveStrategy : IMovementStrategy, IPattern
    {
        public int Range { get; private set; }

        public StraightMoveStrategy(int range)
        {
            Range = range;
        }

        public List<Hex> CalcDestinations(CubeIndex startingPos, GameGrid grid)
        {
            return grid.GetTiles(CubeIndex.GetRadialLine(startingPos, 1, Range));
        }

        public List<CubeIndex> CalcTargets(CubeIndex startingPos, HexDir dir, GameGrid grid)
        {
            return CubeIndex.GetRadialLine(startingPos, 1, Range);
        }
    }
}