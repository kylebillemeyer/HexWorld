using UnityEngine;
using UnityEditor;
using HexWord.Graph;
using System.Collections.Generic;
using HexWorld.Components;

namespace HexWorld.Graph.Movement
{
    public class Ring : IPattern
    {
        public int Radius { get; private set; }
        public int Thickness { get; private set; }

        public Ring(int radius, int thickness)
        {
            this.Radius = radius;
            this.Thickness = thickness;
        }

        public List<CubeIndex> CalcTargets(CubeIndex startingPos, CubeIndex targetPos, GameGrid grid)
        {
            var facingDir = HexDirUtil.fromTargetPos(startingPos, targetPos);
            return CubeIndex.GetRing(startingPos, Radius, Thickness);
        }
    }
}