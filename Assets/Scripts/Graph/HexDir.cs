using UnityEngine;
using UnityEditor;
using HexWorld.Graph;
using HexWord.Graph;
using HexWorld.Util;

namespace HexWord.Graph
{
    public enum HexDir
    {
        Northeast = 0,
        East = 1,
        Southeast = 2,
        Southwest = 3,
        West = 4,
        Northwest = 5
    }

    public static class HexDirUtil
    {
        public static readonly BiDictionary<HexDir, CubeIndex> DirMap = new BiDictionary<HexDir, CubeIndex>();

        static HexDirUtil()
        {
            DirMap.Add(HexDir.Northeast, new CubeIndex(+1, -1, 0));
            DirMap.Add(HexDir.East, new CubeIndex(+1, 0, -1));
            DirMap.Add(HexDir.Southeast, new CubeIndex(0, +1, -1));
            DirMap.Add(HexDir.Southwest, new CubeIndex(-1, +1, 0));
            DirMap.Add(HexDir.West, new CubeIndex(-1, 0, +1));
            DirMap.Add(HexDir.Northwest, new CubeIndex(0, -1, +1));
        }

        public static HexDir rotateRight(this HexDir dir, int amount)
        {
            var dirInt = (int)dir;
            var rotatedInt = wrapIndex(dirInt + amount, 6);
            return (HexDir)rotatedInt;
        }

        public static HexDir rotateLeft(this HexDir dir, int amount)
        {
            return dir.rotateRight(-amount);
        }

        public static int wrapIndex(int i, int i_max)
        {
            return ((i % i_max) + i_max) % i_max;
        }

        public static HexDir fromNeighbor(CubeIndex start, CubeIndex neighbor)
        {
            var offset = neighbor - start;

            return DirMap.Reverse[offset];
        }

        public static HexDir fromTargetPos(CubeIndex startPos, CubeIndex targetPos)
        {
            var facingNeighbor = CubeIndex.nearestIntersectingNeighbor(startPos, targetPos);
            return HexDirUtil.fromNeighbor(startPos, facingNeighbor);
        }
    }
}