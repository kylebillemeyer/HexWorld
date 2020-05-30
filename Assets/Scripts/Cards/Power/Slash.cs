using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld.Cards.Power
{
    public class Slash : Card
    {
        [SerializeField]
        private int damage;
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public Slash()
        {
            Damage = 30;
            RefId = "Slash";
            CardName = "Slash";
            Type = CardType.Power;
            PowerCost = 1;
            Description = "Attach all units in range for 30 damage.";
        }
    }
}