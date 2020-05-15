using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HexWorld.Util;
using HexWorld.Graph;

namespace HexWorld.Components
{
    public class GameGrid : MonoBehaviour
    {
        [SerializeField]
        private int radius;
        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public BiDictionary<CubeIndex, Hex> Tiles { get; set; }
        public BiDictionary<CubeIndex, Unit> Units { get; set; }

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
