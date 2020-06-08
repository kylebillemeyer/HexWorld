using UnityEngine;
using UnityEditor;
using HexWord.Graph;
using System.Collections.Generic;
using HexWorld.Components;
using System;

namespace HexWorld.Graph.Movement
{
    public class Star : IPattern
    {
        public int Radius { get; private set; }

        public Star(int radius)
        {
            this.Radius = radius;
        }

        public List<CubeIndex> CalcTargets(CubeIndex startingPos, CubeIndex targetPos, GameGrid grid)
        {
            List<CubeIndex> cubeIndices = new List<CubeIndex>();
            foreach (var dir in (HexDir[]) Enum.GetValues(typeof(HexDir)))
            {
                cubeIndices.AddRange(CubeIndex.GetLine(startingPos, dir, Radius));
            }

            return cubeIndices;
        }
    }
}