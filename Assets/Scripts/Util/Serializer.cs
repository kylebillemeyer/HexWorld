using UnityEngine;
using UnityEditor;
using System.IO;
using HexWorld.Models;

namespace HexWorld.Util
{
    public class Serializer
    {
        public static readonly string DATA_PATH_PREFIX = "Assets/Data/";

        public static void Serialize(string path, GameGrid grid)
        {
            var json = JsonUtility.ToJson(grid);
            File.WriteAllText(path, json);
        }

        public static GameGrid Deserialize(string path)
        {
            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameGrid>(json);
        }
    }
}