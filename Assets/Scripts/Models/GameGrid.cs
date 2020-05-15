using LanguageExt;
using System.Collections.Generic;

namespace HexWorld.Models
{
    public struct GameGrid
    {
        public Lst<Tile> Tiles { get; }
        public Lst<Unit> Units { get; }
        public Lst<Structure> Structures { get; }
    }
}