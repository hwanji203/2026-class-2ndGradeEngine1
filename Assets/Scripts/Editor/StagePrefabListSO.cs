using System.Collections.Generic;
using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(fileName = "Stage prefab list", menuName = "SO/Stage Prefab", order = 0)]
    public class StagePrefabListSO : ScriptableObject
    {
        public List<GameObject> prefabs = new();
    }
}