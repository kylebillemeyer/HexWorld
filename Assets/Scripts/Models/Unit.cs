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
    }
}