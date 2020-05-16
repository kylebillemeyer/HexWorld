using UnityEngine;
using UnityEditor;
using System;

namespace HexWorld.Models
{
    [Serializable]
    public struct Tile
    {
        public Qub pos;
        public int height;
        public Terrain terrain;
    }
}