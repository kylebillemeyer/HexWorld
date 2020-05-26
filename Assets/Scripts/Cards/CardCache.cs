using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CardCache
{
    public static readonly string FAB_PATH = "Assets/Cards/Prefabs/Instances/";

    public static Dictionary<string, GameObject> Prefabs { get; private set; }

    static CardCache()
    {
        Prefabs = LoadPrefabs();
    }

    private static Dictionary<string, GameObject> LoadPrefabs()
    {
        var fabPaths = Directory.GetFiles(FAB_PATH);
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
        var refId = split[split.Length - 2];
        return refId;
    }
}