using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using System;
using System.Linq;

using HexWorld.LevelEditor;
using HexWorld.Components;

namespace HexWorld.Components
{
    public class LevelEditor : MonoBehaviour
    {
        public static readonly string UNIT_PATH = "Assets/Units/";

        [SerializeField]
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public GameWorld GameWorld { get; private set; }
        public Dictionary<string, GameObject> UnitFabs { get; private set; }

        private Canvas canvas;
        private Brush activeBrush;

        // Use this for initialization
        void Start()
        {
            if (IsEnabled)
            {
                canvas = GetComponentInChildren<Canvas>();

                GameWorld = GameObject.FindObjectOfType<GameWorld>();
                GameWorld.Disabled = true;

                UnitFabs = LoadUnitFabs();

                PlaceButtons();
            }
        }

        private Dictionary<string, GameObject> LoadUnitFabs()
        {
            var unitPath = Directory.GetFiles(UNIT_PATH);
            return unitPath
                .Where(x => x.EndsWith(".prefab"))
                .ToDictionary(x => x, x =>
                {
                    var d = AssetDatabase.LoadAssetAtPath<GameObject>(x);
                    return d;
                });
        }

        private void PlaceButtons()
        {
            var btnFab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UI/TileButton.prefab");
            var btnRect = btnFab.GetComponent<RectTransform>().rect;
            var height = btnRect.height;
            var width = btnRect.width;

            var canRect = canvas.GetComponent<RectTransform>().rect;
            var anchor_x = canRect.xMin + width / 2;
            var anchor_y = canRect.yMax - height / 2;

            var i = 0;
            foreach (KeyValuePair<Terrain, Material> mat in TerrainCache.Cache)
            {
                PlaceButton(mat.Value.name, btnFab, anchor_x, anchor_y - i * height, mat.Value.color, mat.Value.name, InvokeMatBrush, mat.Key);
                i++;
            }

            i++;
            PlaceButton("raise", btnFab, anchor_x, anchor_y - i * height, Color.gray, "Raise", InvokeHeightBrush, 1);
            i++;
            PlaceButton("lower", btnFab, anchor_x, anchor_y - i * height, Color.gray, "Lower", InvokeHeightBrush, -1);

            i++;
            foreach (KeyValuePair<string, GameObject> fab in UnitFabs)
            {
                PlaceButton(fab.Key, btnFab, anchor_x, anchor_y - i * height, Color.white, fab.Value.name, InvokeUnitPlacement, fab.Value);
                i++;
            }

            anchor_x = canRect.xMax - width / 2;
            i = 0;

            PlaceButton<object>("new", btnFab, anchor_x, anchor_y - i * height, Color.gray, "New", InvokeNew, null);
            i++;
            PlaceButton<object>("save", btnFab, anchor_x, anchor_y - i * height, Color.gray, "Save", InvokeSave, null);
            i++;
            PlaceButton<object>("load", btnFab, anchor_x, anchor_y - i * height, Color.gray, "Load", InvokeLoad, null);

            i++;
            i++;
            PlaceButton<object>("battle", btnFab, anchor_x, anchor_y - i * height, Color.gray, "Battle", InvokeBattle, null);
        }

        private void PlaceButton<T>(string id, GameObject prefab, float x, float y, Color color, string text, Action<string, GameObject, T> action, T actionParam)
        {
            var btnObj = Instantiate<GameObject>(prefab, canvas.transform, false);

            var btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => action(id, btnObj, actionParam));
            var cb = btn.colors;
            cb.normalColor = color;
            cb.highlightedColor = Color.white;
            cb.pressedColor = Color.white;
            cb.selectedColor = Color.white;
            btn.colors = cb;

            var trans = btnObj.GetComponent<RectTransform>();
            trans.localPosition = new Vector3(x, y, 0);

            var img = btnObj.GetComponent<Image>();
            //img.color = color;

            var txt = btnObj.GetComponentInChildren<Text>();
            txt.text = text;
        }

        private void InvokeMatBrush(string btnId, GameObject btn, Terrain terrain)
        {
            activeBrush = new MatBrush(terrain, GameWorld.Grid, Camera.main);
        }

        private void InvokeHeightBrush(string btnId, GameObject btn, int dir)
        {
            activeBrush = new HeightBrush(dir, GameWorld.Grid, Camera.main);
        }

        private void InvokeUnitPlacement(string btnId, GameObject btn, GameObject unitFab)
        {
            activeBrush = new UnitBrush(unitFab, GameWorld.Grid, Camera.main);
        }

        private void InvokeNew(string btnId, GameObject btn, object blank)
        {
            GameWorld.New(10);
        }

        private void InvokeSave(string btnId, GameObject btn, object blank)
        {
            GameWorld.Save("test_level");
        }

        private void InvokeLoad(string btnId, GameObject btn, object blank)
        {
            GameWorld.Load("test_level");
        }

        private void InvokeBattle(string btnId, GameObject btn, object blank)
        {
            GameWorld.Disabled = false;
            GameWorld.StartBattle();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsEnabled)
            {
                if (activeBrush != null)
                {
                    activeBrush.Update();
                }
            }
        }
    }
}
