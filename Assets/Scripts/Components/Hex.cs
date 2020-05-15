using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexWorld.Components
{
    public class Hex : MonoBehaviour
    {
        public static double Size = 1;

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

        public Material OrigMaterial { get; set; }

        private Renderer renderer;

        void Start()
        {
            renderer = GetComponent<Renderer>();
            OrigMaterial = renderer.material;

            UpdateHeight(Height == 0 ? Height : 1);
        }

        public Vector3 GetTop()
        {
            var pos = this.gameObject.transform.position;
            return new Vector3(pos.x, pos.y + this.Height * .4f, pos.z);
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