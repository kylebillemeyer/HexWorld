﻿using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;
using HexWorld.Util;
using System.Linq;
using HexWorld.Models;
using HexWorld.Graph;
using HexWorld.Components.Tile;
using HexWorld.Cards;
using HexWord.Battle;
using HexWord.Util;
using HexWorld.Cards.Power;
using HexWord.Battle.States;

namespace HexWorld.Components
{
    public class GameWorld : MonoBehaviour
    {
        public bool Disabled { get; set; }
        public GameGrid Grid { get; set; }
        public Deck Deck { get; set; }
        public StateMachine Machine { get; set; }

        private Camera main_camera;
        private Material selected_mat;
        private Material destination_mat;

        private Hex selection;
        private Hex previousSelection;
        private List<Hex> potentialDestinations;

        public GameWorld()
        {
            Machine = new StateMachine();
        }


        // Use this for initialization
        void Start()
        {
            main_camera = Camera.main;
            selected_mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Selected.mat");
            destination_mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Destination.mat");

            Grid = GetComponentInChildren<GameGrid>();
            Deck = new Deck();
            Deck.AddRange(new List<Card>()
            {
                new Slash(),
                new Slash(),
                new Slash(),
                new Slash(),
                new Slash(),
                new Slash(),
                new Slash(),
                new Slash(),
                new Slash(),
                new Slash(),
                new Slash(),
                new Slash()
            });
        }

        // Update is called once per frame
        void Update()
        {
            if (Disabled)
            {
                return;
            }

            Machine.Update(this);

            if (false)//Input.GetMouseButtonDown(0))
            {
                ResetPreviousSelections();
                selection = Grid.RayDetect(main_camera, PhysicsLayers.Tile).GetComponent<Hex>();

                if (selection != null)
                {
                    HandleNewSelection();
                }
            }
        }

        private void ResetPreviousSelections()
        {
            if (selection != null)
            {
                selection.ResetMaterial();
                previousSelection = selection;
            }
        }

        private void ResetPossibleDestinations()
        {
            if (potentialDestinations != null)
            {
                potentialDestinations.ForEach((d) => d.ResetMaterial());
                potentialDestinations = null;
            }
        }

        private void HandleNewSelection()
        {
            if (potentialDestinations != null && potentialDestinations.Contains(selection))
            {
                var src = Grid.Tiles.Reverse[previousSelection];
                var dest = Grid.Tiles.Reverse[selection];
                Grid.MoveUnit(src, dest);
                ResetPossibleDestinations();
            }
            else if (potentialDestinations != null)
            {
                ResetPossibleDestinations();
            }
            else
            {
                selection.UpdateMaterial(selected_mat);

                if (selection.Unit != null)
                {
                    var selectedIndex = Grid.Tiles.Reverse[selection];

                    var moveStrat = selection.Unit.GetMovementStrategy();
                    this.potentialDestinations = moveStrat.CalcDestinations(selectedIndex, Grid);

                    foreach (var tile in this.potentialDestinations)
                    {
                        tile.UpdateMaterial(destination_mat);
                    }
                }
            }
        }

        internal void New(int radius)
        {
            Grid.Initialize(radius);
        }

        public void Load(string levelName)
        {
            var path = LevelNameToPath(levelName);
            var gridData = Serializer.Deserialize(path);

            if (gridData.rotatorPos != null)
            {
                main_camera.transform.parent.position = gridData.rotatorPos;
                main_camera.transform.localPosition = gridData.cameraLocalPos;
                main_camera.transform.LookAt(main_camera.transform.parent.position);
            }


            Grid.Initialize(gridData);
        }

        public void Save(string levelName)
        {
            var path = LevelNameToPath(levelName);

            var tiles = Grid.Tiles.Forward
                .Select(pair => new Models.Tile() 
                { 
                    pos = pair.Key.ToQub(), 
                    height = pair.Value.Height, 
                    terrain = pair.Value.Terrain 
                })
                .ToList();

            var units = Grid.PlayerUnits.Forward
                .Select(pair => new Models.Unit()
                {
                    pos = pair.Key.ToQub(),
                    range = pair.Value.Range,
                    health = pair.Value.Health,
                    maxHealth = pair.Value.MaxHealth,
                    mana = pair.Value.Mana,
                    maxMana = pair.Value.MaxMana,
                    power = pair.Value.Power,
                    focus = pair.Value.Focus
                })
                .ToList();

            var data = new Models.GameGrid()
            {
                tiles = tiles,
                units = units,
                structures = new List<Structure>(),
                rotatorPos = main_camera.transform.parent.position,
                cameraLocalPos = main_camera.transform.localPosition,
            };

            Serializer.Serialize(path, data);
        }

        private string LevelNameToPath(string levelName)
        {
            return Serializer.DATA_PATH_PREFIX + levelName + ".json";
        }

        public void StartBattle()
        {
            Machine.ChangeState(new BattleStart(Machine));
        }
    }
}
