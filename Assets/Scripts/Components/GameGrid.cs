using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using HexWorld.Util;
using HexWorld.Graph;
using HexWorld.Models;
using HexWorld.Components.Tile;

namespace HexWorld.Components
{
    public class GameGrid : MonoBehaviour
    {
        public BiDictionary<CubeIndex, Hex> Tiles { get; set; }
        public BiDictionary<CubeIndex, Unit> Units { get; set; }

        private GameObject hexFab;
        private GameObject unitFab;

        // Start is called before the first frame update
        void Start()
        {
            hexFab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/hex.prefab");
            unitFab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Units/unit.prefab");
            Reset();
        }

        private void Reset()
        {
            Tiles = new BiDictionary<CubeIndex, Hex>();
            Units = new BiDictionary<CubeIndex, Unit>();
        }

        public void Initialize(Models.GameGrid gridData)
        {
            Reset();
            gridData.tiles.ForEach(tile =>
            {
                CreateHexAtPos(CubeIndex.FromQub(tile.pos), tile.terrain);
            });

            gridData.units.ForEach(unit =>
            {
                CreateUnitAtPos(CubeIndex.FromQub(unit.pos), unit.range);
            });
        }

        public void Initialize(int radius)
        {
            Reset();
            var center = new CubeIndex(0, 0, 0);

            var spiral = CubeIndex.GetSpiral(center, radius);
            foreach (var index in spiral)
            {
                CreateHexAtPos(index, Terrain.None);
            }
        }

        private void CreateHexAtPos(CubeIndex index, Terrain terrain)
        {
            var hexInst = Instantiate(hexFab);
            hexInst.gameObject.transform.position = index.Position();

            var hex = hexInst.GetComponent<Hex>();
            hex.Terrain = terrain;

            Tiles.Add(index, hex);
        }

        private void CreateUnitAtPos(CubeIndex index, int range)
        {
            var unitInst = Instantiate(unitFab);
            var unit = unitInst.GetComponent<Unit>();
            unit.Range = range;
            PlaceUnit(unit, index);
        }

        public void PlaceUnit(Unit unit, CubeIndex dest)
        {
            var tile = Tiles.Forward[dest];
            tile.Unit = unit;
            unit.gameObject.transform.position = tile.GetTop();

            Units.Add(dest, unit);
        }

        public void MoveUnit(CubeIndex src, CubeIndex dest)
        {
            var srcTile = Tiles.Forward[src];
            var unit = srcTile.Unit;
            srcTile.Unit = null;
            PlaceUnit(unit, dest);

            Units.Remove(src);
        }

        public List<Hex> GetTiles(IEnumerable<CubeIndex> indices)
        {
            return indices
                .Where((x) => Tiles.Forward.ContainsKey(x))
                .Select((x) => Tiles.Forward[x])
                .ToList<Hex>();
        }

        public Hex RayDetectHex(Camera camera)
        {
            int layerMask = 1 << 8;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                return hit.collider.gameObject.GetComponent<Hex>();
            }
            else
            {
                return null;
            }
        }
    }
}
