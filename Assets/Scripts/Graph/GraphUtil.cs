using UnityEngine;
using UnityEditor;
using HexWorld.Graph;
using HexWord.Graph;

namespace HexWord.Graph
{
    public static class GraphUtil
    {
        public static CubeIndex AsInd(this HexDir dir)
        {
            switch (dir)
            {
                case HexDir.Northeast:
                    return new CubeIndex(+1, -1, 0);
                case HexDir.East:
                    return new CubeIndex(+1, 0, -1);
                case HexDir.Southeast:
                    return new CubeIndex(0, +1, -1);
                case HexDir.Southwest:
                    return new CubeIndex(-1, +1, 0);
                case HexDir.West:
                    return new CubeIndex(-1, 0, +1);
                default: // Northwest
                    return new CubeIndex(0, -1, +1);
            }
        }
    }
}