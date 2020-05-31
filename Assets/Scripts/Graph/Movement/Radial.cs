using HexWord.Graph;
using HexWorld.Components;
using HexWorld.Components.Tile;
using HexWorld.Graph;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld.Graph.Movement
{
    public class Radial : IMovementStrategy, IPattern
    {
        public int Range { get; private set; }

        public Radial(int range)
        {
            Range = range;
        }

        public List<Hex> CalcDestinations(CubeIndex startingPos, GameGrid grid)
        {
            return grid.GetTiles(CubeIndex.GetSpiral(startingPos, Range));
        }

        public List<CubeIndex> CalcTargets(CubeIndex startingPos, HexDir dir, GameGrid grid)
        {
            return CubeIndex.GetSpiral(startingPos, Range);
        }
    }
}
