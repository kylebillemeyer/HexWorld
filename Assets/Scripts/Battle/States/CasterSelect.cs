using UnityEngine;
using UnityEditor;
using HexWord.Util;
using HexWorld.Components;
using System.Linq;
using HexWorld.Cards;
using System;

namespace HexWord.Battle.States
{
    public class CasterSelect : State
    {
        private readonly Card selectedCard;

        public CasterSelect(Card selectedCard, StateMachine machine) : base(machine) {
            this.selectedCard = selectedCard;
        }

        public override void OnEnter(GameWorld world)
        {
            var validChoices = world.Grid.PlayerUnits.Forward.Values
                .Where(unit => CanCast(selectedCard, unit))
                .ToList();

            foreach (var unit in validChoices)
            {
                var occupiedTile = world.Grid.Tiles.Forward[world.Grid.PlayerUnits.Reverse[unit]];
                //var highlight = Instantiate()
            }
        }

        private bool CanCast(Card card, Unit unit)
        {
            return card.PowerCost <= unit.Power &&
                card.FocusCost <= unit.Focus &&
                card.SoulCost <= unit.Soul &&
                card.SpiritCost <= unit.Spirit;
        }

        public override void Update(GameWorld world)
        {
            
        }
    }
}
