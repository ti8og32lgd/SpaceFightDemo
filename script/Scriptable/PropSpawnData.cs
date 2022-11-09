using System;
using System.Collections.Generic;
using UnityEngine;

namespace script.Scriptable
{    
    [CreateAssetMenu(fileName = "PropSpawnData", menuName = "data/PropSpawnData", order = 0)]
    public class PropSpawnData : ScriptableObject
    {

        [Serializable]
        public struct PropsWeightUnit
        {
            public PropData data;
            public float weight;
        } 
        public List<PropsWeightUnit> PropList;

    }
}