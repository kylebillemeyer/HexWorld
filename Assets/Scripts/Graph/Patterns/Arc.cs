using UnityEngine;
using UnityEditor;
using HexWord.Graph;
using System.Collections.Generic;
using HexWorld.Components.Tile;
using HexWorld.Graph;
using HexWorld.Components;

public class Arc : IPattern
{
    public int ImmediateArcWidth { get; private set; }
    public int ArcDepth { get; private set; }

    public Arc(int immediateArcWidth, int arcDepth)
    {
        ImmediateArcWidth = immediateArcWidth;
        ArcDepth = arcDepth;
    }

    public List<CubeIndex> CalcTargets(CubeIndex startingPos, HexDir dir, GameGrid grid)
    {
        var targets = new List<CubeIndex>();

        // This function assumes dir is the center of the arc for odd numbers and the center-right (clock-wise) hex for even numbers.
        int distToRightmost = (ImmediateArcWidth - 1) / 2;

        var rightmostDir = dir.rotateRight(distToRightmost);
        var start = startingPos.GetNeighbor(rightmostDir);

        var current_depth = 1;
        var current_tile = start;
        while (current_depth <= ArcDepth)
        {
            var i = 0;
            while (i < ImmediateArcWidth)
            {
                var current_dir = rightmostDir.rotateLeft(2 + i);

                var j = 0;
                while (j < current_depth)
                {
                    targets.Add(current_tile);
                    current_tile = current_tile.GetNeighbor(current_dir);
                    j++;
                }

                i++;
            }
            
            current_depth++;
            start.GetNeighbor(rightmostDir);
            current_tile = start;
        }

        return targets;
    }
}