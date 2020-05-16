using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;

public static class TerrainCache
{
    public static readonly string MAT_PATH = "Assets/Materials/Tiles/";

    public static Dictionary<Terrain, Material> Cache { get; private set; }

    static TerrainCache()
    {
        Cache = LoadMaterials();
    }
    private static Dictionary<Terrain, Material> LoadMaterials()
    {
        var matPaths = Directory.GetFiles(MAT_PATH);
        return matPaths
            .Where(x => x.EndsWith(".mat"))
            .ToDictionary(
                x => MapName(x), 
                x => AssetDatabase.LoadAssetAtPath<Material>(x)
            );
    }

    private static Terrain MapName(string path)
    {
        var split = path.Split('/', '.');
        var name = split[split.Length-2];
        return (Terrain)Enum.Parse(typeof(Terrain), name, true);
    }
}