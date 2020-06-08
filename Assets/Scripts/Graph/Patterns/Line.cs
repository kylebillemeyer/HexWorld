using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using HexWorld.Components;
using HexWorld.Components.Tile;
using HexWorld.Graph;
using HexWord.Graph;

namespace HexWorld.Graph.Movement
{
    public class Line : IMovementStrategy, IPattern
    {
        public int Range { get; private set; }

        public Line(int range)
        {
            Range = range;
        }

        public List<Hex> CalcDestinations(CubeIndex startingPos, GameGrid grid)
        {
            return grid.GetTiles(CubeIndex.GetLine(startingPos, HexDir.East, Range));
        }

        public List<CubeIndex> CalcTargets(CubeIndex startingPos, CubeIndex targetPos, GameGrid grid)
        {
            var facingDir = HexDirUtil.fromTargetPos(startingPos, targetPos);
            return CubeIndex.GetLine(startingPos, facingDir, Range);
        }
    }
}