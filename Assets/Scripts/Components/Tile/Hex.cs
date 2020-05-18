using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexWorld.Components.Tile
{
    public class Hex : MonoBehaviour
    {
        public static double Radius = 1;

        [SerializeField]
        private Unit unit;
        public Unit Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        [SerializeField]
        private int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        [SerializeField]
        private Terrain terrain;
        public Terrain Terrain { 
            get { return terrain; }
            set { terrain = value; }
        }

        private Renderer renderer;

        void Start()
        {
            renderer = GetComponent<Renderer>();

            ResetMaterial();

            UpdateHeight(Height == 0 ? 1 : Height);
        }

        public Vector3 GetTop()
        {
            var pos = this.gameObject.transform.position;
            return new Vector3(pos.x, pos.y + this.Height * .4f, pos.z);
        }

        public void ResetMaterial()
        {
            UpdateMaterial(TerrainCache.Cache[Terrain]);
        }

        public void UpdateMaterial(Material material)
        {
            renderer.material = material;
        }

        public void UpdateHeight(int height)
        {
            if (height < 1)
            {
                return;
            }

            Height = height;
            var ls = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(ls.x, 2 * Height, ls.z);
        }
    }
}