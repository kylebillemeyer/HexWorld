using UnityEngine;
using UnityEditor;
using HexWord.Graph;
using System.Collections.Generic;
using HexWorld.Graph;
using HexWorld.Components;

public class DetachedRadial : IPattern
{
    public int Radius { get; private set; }

    public DetachedRadial(int radius)
    {
        this.Radius = radius;
    }

    public List<CubeIndex> CalcTargets(CubeIndex startingPos, CubeIndex targetPos, GameGrid grid)
    {
        return CubeIndex.GetSpiral(targetPos, Radius, true);
    }
}