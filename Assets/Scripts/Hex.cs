using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public static double Size = .5;

    [SerializeField]
    private Unit unit;
    public Unit Unit
    {
        get { return unit; }
        set { unit = value; }
    }

    public Material OrigMaterial { get; set; }

    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        OrigMaterial = renderer.material;
        //AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Selected.mat");
    }

    public void UpdateMaterial(Material material)
    {
        renderer.material = material;
    }
}