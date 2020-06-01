using HexWorld.Components;
using HexWorld.Components.Tile;
using System;
using UnityEngine;

namespace HexWorld.Graph
{
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
            return new CubeIndex(Q, -Q - R, R);
        }

        public Vector3 Position()
        {
            return new Vector3(
                (float)(Hex.Radius * (2 * Q + R)),
                0,
                (float)(Hex.Radius * 1.5 * R));
        }

        public static AxialIndex FromPosition(Vector3 pos)
        {
            var q = (int)Math.Floor((pos.x / 2 - 1.0 / 3 * pos.z) / Hex.Radius);
            var r = (int)Math.Floor((2.0 / 3 * pos.z) / Hex.Radius);

            return new AxialIndex(q, r);
        }
    }
}
