using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int radius;
    public int Radius
    {
        get { return radius; }
        set { radius = value;  }
    }

    public BiDictionary<CubeIndex, Hex> Tiles { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Tiles = new BiDictionary<CubeIndex, Hex>();

        var hexFab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/hex.prefab");
        var center = new CubeIndex(0, 0, 0);

        var spiral = CubeIndex.GetSpiral(center, Radius);
        foreach (var index in spiral)
        {
            var pos = index.Position();
            var hexInst = (GameObject)Instantiate(hexFab);
            hexInst.gameObject.transform.position = pos;

            var hex = hexInst.GetComponent<Hex>();
            Tiles.Add(index, hex);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceUnit(Unit unit, CubeIndex dest)
    {
        var tile = Tiles.Forward[dest];
        tile.Unit = unit;
        unit.gameObject.transform.position = tile.gameObject.transform.position;
    }

    public void MoveUnit(CubeIndex src, CubeIndex dest)
    {
        var srcTile = Tiles.Forward[src];
        var unit = srcTile.Unit;
        srcTile.Unit = null;
        PlaceUnit(unit, dest);
    }

    public List<Hex> GetTiles(IEnumerable<CubeIndex> indices)
    {
        return indices.Select((x) => Tiles.Forward[x]).ToList<Hex>();
    }
}
