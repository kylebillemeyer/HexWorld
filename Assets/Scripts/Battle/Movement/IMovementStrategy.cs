using HexWorld.Components;
using HexWorld.Components.Tile;
using HexWorld.Graph;
using System.Collections;
using System.Collections.Generic;

namespace HexWorld.Movement
{
    public interface IMovementStrategy
    {
        List<Hex> CalcDestinations(CubeIndex startingPos, GameGrid grid);
    }
}