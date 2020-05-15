using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using System;
using System.Linq;

using HexWorld.Editor;
using HexWorld.Components;

namespace HexWorld.Components
{
    public class LevelEditor : MonoBehaviour
    {
        public static readonly string MAT_PATH = "Assets/Materials/Tiles/";
        public static readonly string UNIT_PATH = "Assets/Units/";

        [SerializeField]
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public GameWorld GameWorld { get; private set; }
        public Dictionary<string, Material> Materials { get; private set; }
        public Dictionary<string, GameObject> UnitFabs { get; private set; }
        public Material NoneMaterial { get; private set; }

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

                Materials = LoadMaterials();
                NoneMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/None.mat");

                UnitFabs = LoadUnitFabs();

                PlaceButtons();
            }
        }

        private Dictionary<string, Material> LoadMaterials()
        {
            var matPaths = Directory.GetFiles(MAT_PATH);
            return matPaths
                .Where(x => x.EndsWith(".mat"))
                .Where(x => !x.Contains("None"))
                .ToDictionary(x => x, x =>
                    AssetDatabase.LoadAssetAtPath<Material>(x)
                );
        }

        private Dictionary<string, GameObject> LoadUnitFabs()
        {
            var unitPath = Directory.GetFiles(UNIT_PATH);
            return unitPath
                .Where(x => x.EndsWith(".prefab"))
                .ToDictionary(x => x, x =>
                    AssetDatabase.LoadAssetAtPath<GameObject>(x)
                );
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
            foreach (KeyValuePair<string, Material> mat in Materials)
            {
                PlaceButton<Material>(mat.Key, btnFab, anchor_x, anchor_y - i * height, mat.Value.color, mat.Value.name, InvokeMatBrush, mat.Value);
                i++;
            }

            i++;
            PlaceButton<int>("raise", btnFab, anchor_x, anchor_y - i * height, Color.gray, "Raise", InvokeHeightBrush, 1);
            i++;
            PlaceButton<int>("lower", btnFab, anchor_x, anchor_y - i * height, Color.gray, "Lower", InvokeHeightBrush, -1);

            i++;
            foreach (KeyValuePair<string, GameObject> fab in UnitFabs)
            {
                PlaceButton(fab.Key, btnFab, anchor_x, anchor_y - i * height, Color.white, fab.Value.name, InvokeUnitPlacement, fab.Value);
                i++;
            }
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

        private void InvokeMatBrush(string btnId, GameObject btn, Material mat)
        {
            activeBrush = new MatBrush(mat, GameWorld.Grid, Camera.main);
        }

        private void InvokeHeightBrush(string btnId, GameObject btn, int dir)
        {
            activeBrush = new HeightBrush(dir, GameWorld.Grid, Camera.main);
        }

        private void InvokeUnitPlacement(string btnId, GameObject btn, GameObject unitFab)
        {
            activeBrush = new UnitBrush(unitFab, GameWorld.Grid, Camera.main);
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
