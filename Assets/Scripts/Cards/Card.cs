using UnityEditor;

namespace HexWorld.Cards
{
    public class Card
    {
        public GUID RefId { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
    }
}