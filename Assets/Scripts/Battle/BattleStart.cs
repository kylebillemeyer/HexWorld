using UnityEngine;
using UnityEditor;
using HexWorld.Components;
using HexWord.Util;
using HexWorld.Battle;

namespace HexWord.Battle
{
    public class BattleStart : State
    {
        private GameObject ui;
        private Timer startTimer;

        public BattleStart(StateMachine machine) : base(machine) 
        {
            ui = GameObject.Find("Canvas").FindObject("BattleStartUI");
            startTimer = ui.GetComponentInChildren<Timer>();
        }

        public override void OnEnter(GameWorld world)
        {
            world.Deck.Reset();
            ui.SetActive(true);
            startTimer.Paused = false;
        }

        public override void Update(GameWorld world)
        {
        }

        public void OnStartTimeout()
        {
            machine.ChangeState(new PlayerPhase(machine));
        }
    }
}