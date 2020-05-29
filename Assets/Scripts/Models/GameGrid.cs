using System.Collections.Generic;
using UnityEngine;

namespace HexWorld.Models
{
    [System.Serializable]
    public struct GameGrid
    {
        public Vector3 rotatorPos;
        public Vector3 cameraLocalPos;
        public List<Tile> tiles;
        public List<Unit> units;
        public List<Structure> structures;
    }
}