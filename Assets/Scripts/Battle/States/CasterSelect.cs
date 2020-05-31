using UnityEngine;
using UnityEditor;
using HexWord.Util;
using HexWorld.Components;
using System.Linq;
using System.Collections.Generic;
using HexWorld.Cards;
using System;
using HexWorld.Components.Tile;
using HexWord.Util;

namespace HexWord.Battle.States
{
    public class CasterSelect : State
    {
        private readonly Card selectedCard;

        private List<Hex> validChoices;

        public CasterSelect(Card selectedCard, StateMachine machine) : base(machine) {
            this.selectedCard = selectedCard;
            this.validChoices = new List<Hex>();
        }

        public override void OnEnter(GameWorld world)
        {
            validChoices = world.Grid.PlayerUnits.Forward.Values
                .Where(unit => CanCast(selectedCard, unit))
                .Select(unit => world.Grid.Tiles.Forward[world.Grid.PlayerUnits.Reverse[unit]])
                .ToList();

            foreach (var tile in validChoices)
            {
                PlaceSelectionEffect(tile);
            }
        }

        public override void OnExit(GameWorld world)
        {
            foreach (var tile in validChoices)
            {
                var effect = tile.gameObject.FindObject("MiddleHexCyber", true);
                UnityEngine.Object.Destroy(effect);
            }
        }

        private void PlaceSelectionEffect(Hex occupiedTile)
        {
            var fab = EffectCache.Prefabs["MiddleHexCyber"];

            var inst = UnityEngine.Object.Instantiate(fab);
            inst.transform.parent = occupiedTile.transform;
            inst.transform.position = occupiedTile.GetTop();
            inst.layer = (int)PhysicsLayers.Selectable;

            var collider = inst.AddComponent<SphereCollider>();
            collider.radius = Hex.Radius;
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
            if (Input.GetMouseButtonDown(0))
            {
                var collider = world.Grid.RayDetect(Camera.main, PhysicsLayers.Selectable);
                if (collider != null)
                {
                    var hex = collider.GetComponentInParent<Hex>();
                    var index = world.Grid.Tiles.Reverse[hex];
                    // if this can't be found, the collider is being placed on the wrong tile.
                    var unit = world.Grid.PlayerUnits.Forward[index];

                    machine.ChangeState(new PatternPlacementSelect(selectedCard, unit, machine));
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                machine.ReturnToPrevious(world);
            }
        }
    }
}
