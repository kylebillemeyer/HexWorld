﻿using HexWorld.Components;
using HexWorld.Components.Tile;
using System.Collections;
using System.Collections.Generic;

namespace HexWorld.Graph
{
    public interface IMovementStrategy
    {
        List<Hex> CalcDestinations(CubeIndex startingPos, GameGrid grid);
    }
}