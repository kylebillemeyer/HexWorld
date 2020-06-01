using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public enum Terrain
{
    None = 0,
    Selection = 1000,
    Pattern = 1001,
    Grass = 1,
    Dirt = 2,
    Sand = 3,
    Water = 4
}