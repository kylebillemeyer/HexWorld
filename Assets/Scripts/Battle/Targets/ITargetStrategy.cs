using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using HexWorld.Components;

namespace Battle.Targets
{
    public interface ITargetStrategy
    {
        List<Unit> GetTargets();
    }
}