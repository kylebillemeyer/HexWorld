using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using HexWorld.Graph;
using HexWorld.Components.Tile;
using HexWorld.Components;

namespace HexWord.Graph
{
    public interface IPattern
    {
        List<CubeIndex> CalcTargets(CubeIndex startingPos, CubeIndex targetPos, GameGrid grid);
    }
}