using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using HexWorld.Components;
using HexWorld.Components.Tile;
using HexWorld.Graph;

namespace HexWorld.Movement
{
    public class StraightMoveStrategy : IMovementStrategy
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
    }
}