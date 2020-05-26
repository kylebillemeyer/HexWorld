using UnityEngine;
using UnityEditor;
using HexWorld.Components;
using HexWord.Util;
using HexWord.Cards;

namespace HexWorld.Battle
{
    public class Draw : State
    {
        public Draw(int numCards, StateMachine machine) : base(machine) { }

        private GameObject handUI;
        private HandUI handUIScript;

        public override void OnEnter(GameWorld world)
        {            
            handUI = GameObject.Find("Canvas").FindObject("HandUI");
            handUIScript = handUI.GetComponent<HandUI>();

            var cards = world.Deck.Draw(6);
            handUIScript.Draw(cards);
        }

        public override void Update(GameWorld world)
        {
            if (!handUIScript.Busy && world.Deck.DrawPile.Count >= 6)
            {
                //handUIScript.Draw(world.Deck.Draw(6));
            }
        }

        public override void OnExit(GameWorld world)
        {
            world.Deck.DiscardHand();
        }
    }
}