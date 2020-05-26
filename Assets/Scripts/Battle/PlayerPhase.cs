using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HexWorld.Components;
using System.Linq;
using HexWord.Util;
using UnityEngine.UI;

namespace HexWorld.Battle
{
    public class PlayerPhase : State
    {
        public static readonly int HAND_SIZE = 6;

        public HashSet<Unit> UnexhaustedUnits { get; set; }

        private GameObject ui;
        private Timer startTimer;

        private bool allowControl;

        public PlayerPhase(StateMachine machine) : base(machine)
        {
            ui = GameObject.Find("Canvas").FindObject("PlayerPhaseUI");
            startTimer = ui.GetComponentInChildren<Timer>();
        }

        public override void OnEnter(GameWorld world)
        {
            world.Deck.Reset();
            ui.SetActive(true);
            startTimer.OnTimeout.AddListener(OnStartTimeout);
            startTimer.Reset();
            startTimer.Paused = false;
        }

        public override void Update(GameWorld world)
        {

        }

        public override void OnExit(GameWorld world)
        {
            startTimer.Paused = true;
            ui.SetActive(false);
        }

        public void OnStartTimeout()
        {
            machine.ChangeState(new Draw(6, machine));
        }
    }
}
