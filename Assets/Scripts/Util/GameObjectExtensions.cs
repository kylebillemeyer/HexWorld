using UnityEngine;
using UnityEditor;

namespace HexWord.Util
{
    public static class GameObjectExtensions
    {
        public static GameObject FindObject(this GameObject parent, string name, bool usePrefixMatch = false)
        {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if ((usePrefixMatch && t.name.StartsWith(name)) || (!usePrefixMatch && t.name == name))
                {
                    return t.gameObject;
                }
            }
            return null;
        }
    }
}