using System;
using System.Collections.Generic;
using UnityEngine;

namespace script.Scriptable
{    
    [CreateAssetMenu(fileName = "RandomEnemySpawnData", menuName = "data/RandomEnemySpawnData", order = 0)]
    public class RandomEnemySpawnData : EnemySpawnData
    {

        
        [Serializable]
        public struct EnemyWeightUnit
        {
            public EnemyData data;
            [Range(0,10)]
            public float weight;
        }

        [Header("spawn interval")]
        public float intervalMin = 1.0f;
        public float intervalMax = 5.0f;

        [Header("setting")] public int countOnceMin = 1;
        public int countOnceMax = 3;
        
        [Header("End condition")] 
        public float spawnEndTime = 10f;
        public int spawnEndRound=20;
        
        [Header("weights")]
        public List<EnemyWeightUnit> enemyWeightList;
    
    }
}