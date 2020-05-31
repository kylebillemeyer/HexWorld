using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class EffectCache
{
    public static readonly string FAB_PATH = "Assets/VFX/Prefabs/";

    public static readonly List<string> secondary_paths = new List<string>()
    {
        "Assets/OrdossFX/SelectionBasesFX/Prefabs/Middle/MiddleHexCyber.prefab"
    };

    public static Dictionary<string, GameObject> Prefabs { get; private set; }

    static EffectCache()
    {
        Prefabs = LoadPrefabs();
    }

    private static Dictionary<string, GameObject> LoadPrefabs()
    {
        var fabPaths = secondary_paths.Concat(Directory.GetFiles(FAB_PATH));
        return fabPaths
            .Where(x => x.EndsWith(".prefab"))
            .ToDictionary(
                x => MapName(x),
                x => AssetDatabase.LoadAssetAtPath<GameObject>(x)
            );
    }

    private static string MapName(string path)
    {
        var split = path.Split('/', '.');
        var name = split[split.Length - 2];
        return name;
    }
}