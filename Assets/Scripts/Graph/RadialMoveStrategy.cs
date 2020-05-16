using HexWorld.Components;
using HexWorld.Components.Tile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld.Graph
{
    public class RadialMoveStrategy : IMovementStrategy
    {
        public int Range { get; private set; }

        public RadialMoveStrategy(int range)
        {
            Range = range;
        }

        public List<Hex> CalcDestinations(CubeIndex startingPos, GameGrid grid)
        {
            return grid.GetTiles(CubeIndex.GetSpiral(startingPos, Range));
        }
    }
}
