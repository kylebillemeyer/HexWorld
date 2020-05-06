using System.Collections;
using System.Collections.Generic;

public interface IMovementStrategy
{
    List<Hex> CalcDestinations(CubeIndex startingPos, Grid grid);
}