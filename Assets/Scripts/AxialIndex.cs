using System;
using UnityEngine;

public class AxialIndex
{
    public int Q { get; set; }
    public int R { get; set; }

    public AxialIndex(int q, int r)
    {
        Q = q;
        R = r;
    }

    public CubeIndex ToCube()
    {
        return new CubeIndex(Q, R, -Q-R);
    }

    public Vector3 Position()
    {
        return new Vector3(
            (float)(Hex.Size * (Math.Sqrt(3) * Q + Math.Sqrt(3) / 2 * R)),
            0,
            (float)(Hex.Size * 1.5 * R));
    }

    public static AxialIndex FromPosition(Vector3 pos)
    {
        var q = (int)Math.Floor((Math.Sqrt(3) / 3 * pos.x - 1.0 / 3 * pos.z) / Hex.Size);
        var r = (int)Math.Floor((2.0 / 3 * pos.z) / Hex.Size);

        return new AxialIndex(q, r);
    }
}
