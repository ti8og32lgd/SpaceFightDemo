using System;
using UnityEngine;

namespace script.ProjectDebug
{
   
    [CreateAssetMenu(fileName = "DebugPrefabLinks", menuName = "ScriptableObjects/DebugPrefabLinks", order = 0)]
    public class DebugObjects : ScriptableObject
    {
        [Serializable]public struct PrefabUnit
        {
            [SerializeField] public string name;
            public GameObject prefab;

            public PrefabUnit(string name, GameObject prefab)
            {
                this.name = name;
                this.prefab = prefab;
            }
        }

        
        public PrefabUnit[] prefabLinks;
    }
}