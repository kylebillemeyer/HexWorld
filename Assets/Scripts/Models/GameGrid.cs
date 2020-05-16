using System.Collections.Generic;
using UnityEngine;

namespace HexWorld.Models
{
    [System.Serializable]
    public struct GameGrid
    {
        public Vector3 cameraPos;
        public Vector3 cameraDir;
        public List<Tile> tiles;
        public List<Unit> units;
        public List<Structure> structures;
    }
}