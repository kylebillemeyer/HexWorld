using UnityEngine;
using UnityEditor;
using HexWorld.Graph;
using HexWord.Graph;

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
    }
}