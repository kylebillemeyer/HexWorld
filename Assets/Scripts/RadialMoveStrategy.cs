using System;
using System.Collections.Generic;
using UnityEngine;

public class RadialMoveStrategy : IMovementStrategy
{
    public int Range { get; private set; }

    public RadialMoveStrategy(int range)
    {
        Range = range;
    }

    public List<Hex> CalcDestinations(CubeIndex startingPos, Grid grid)
    {
        return grid.GetTiles(CubeIndex.GetSpiral(startingPos, Range));
    }
}
