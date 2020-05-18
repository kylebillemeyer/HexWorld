using UnityEngine;
using UnityEditor;

namespace HexWorld.Models
{
    [System.Serializable]
    public class Unit
    {
        public Qub pos;
        public int range;
        public int health;
        public int maxHealth;
        public int mana;
        public int maxMana;
        public int power;
        public int focus;
    }
}