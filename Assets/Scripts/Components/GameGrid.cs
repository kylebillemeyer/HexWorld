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
        public BiDictionary<CubeIndex, Unit> PlayerUnits { get; set; }

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
            PlayerUnits = new BiDictionary<CubeIndex, Unit>();
        }

        public void Initialize(Models.GameGrid gridData)
        {
            Reset();
            gridData.tiles.ForEach(tile =>
            {
                CreateHexAtPos(tile);
            });

            gridData.units.ForEach(unit =>
            {
                CreateUnitAtPos(unit);
            });
        }

        public void Initialize(int radius)
        {
            Reset();
            var center = new CubeIndex(0, 0, 0);

            var spiral = CubeIndex.GetSpiral(center, radius);
            foreach (var index in spiral)
            {
                CreateHexAtPos(new Models.Tile() { pos = index.ToQub(), terrain = Terrain.None, height = 1 });
            }
        }

        private void CreateHexAtPos(Models.Tile data)
        {
            var index = CubeIndex.FromQub(data.pos);

            var hexInst = Instantiate(hexFab);
            hexInst.gameObject.transform.position = index.Position();

            var hex = hexInst.GetComponent<Hex>();
            hex.Terrain = data.terrain;
            hex.UpdateHeight(data.height);

            Tiles.Add(index, hex);
        }

        private void CreateUnitAtPos(Models.Unit data)
        {
            var index = CubeIndex.FromQub(data.pos);

            var unitInst = Instantiate(unitFab);
            var unit = unitInst.GetComponent<Unit>();
            unit.Range = data.range;
            unit.Health = data.health;
            unit.MaxHealth = data.maxHealth;
            unit.Mana = data.mana;
            unit.MaxMana = data.maxMana;
            unit.Power = data.power;
            unit.Focus = data.focus;
            PlaceUnit(unit, index);
        }

        public void PlaceUnit(Unit unit, CubeIndex dest)
        {
            var tile = Tiles.Forward[dest];
            tile.Unit = unit;
            unit.gameObject.transform.position = tile.GetTop();

            PlayerUnits.Add(dest, unit);
        }

        public void MoveUnit(CubeIndex src, CubeIndex dest)
        {
            var srcTile = Tiles.Forward[src];
            var unit = srcTile.Unit;
            srcTile.Unit = null;
            PlaceUnit(unit, dest);

            PlayerUnits.Remove(src);
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
