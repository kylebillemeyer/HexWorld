using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using System;
using System.Linq;

public class LevelEditor : MonoBehaviour
{
    public static readonly string MAT_PATH = "Assets/Materials/Tiles/";

    public GameWorld GameWorld{ get; private set; }
    public Dictionary<string, Material> Materials { get; private set; }
    public Material NoneMaterial { get; private set; }

    private Canvas canvas;
    private Brush activeBrush;

    // Use this for initialization
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();

        GameWorld = GameObject.FindObjectOfType<GameWorld>();
        GameWorld.Disabled = true;

        Materials = LoadMaterials();
        NoneMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/None.mat");

        PlaceButtons();
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

    private void PlaceButtons()
    {
        var btnFab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UI/TileButton.prefab");
        var btnRect = btnFab.GetComponent<RectTransform>().rect;
        var height = btnRect.height;
        var width = btnRect.width;

        var canRect = canvas.GetComponent<RectTransform>().rect;
        var anchor_x = canRect.xMin + width/2;
        var anchor_y = canRect.yMax - height/2;
   
        var i = 0;
        foreach (KeyValuePair<string, Material> mat in Materials)
        {
            PlaceButton(mat.Key, btnFab, anchor_x, anchor_y - i * height, mat.Value.color, mat.Value.name, InvokePaint);
            i++;
        }

        i++;
        PlaceButton("raise", btnFab, anchor_x, anchor_y - i * height, Color.gray, "Raise", InvokeRaise);
        i++;
        PlaceButton("lower", btnFab, anchor_x, anchor_y - i * height, Color.gray, "Lower", InvokeLower);
    }

    private void PlaceButton(string id, GameObject prefab, float x, float y, Color color, string text, Action<string, GameObject> action)
    {
        var btnObj = Instantiate<GameObject>(prefab, canvas.transform, false);

        var btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(() => action(id, btnObj));
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

    private void InvokePaint(string btnId, GameObject btn)
    {
        activeBrush = new MatBrush(Materials[btnId], GameWorld.Grid, Camera.main);
    }

    private void InvokeRaise(string btnId, GameObject btn)
    {
        activeBrush = new HeightBrush(1, GameWorld.Grid, Camera.main);
    }

    private void InvokeLower(string btnId, GameObject btn)
    {
        activeBrush = new HeightBrush(-1, GameWorld.Grid, Camera.main);
    }

    // Update is called once per frame
    void Update()
    {
        if (activeBrush != null)
        {
            activeBrush.Update();
        }
    }
}
