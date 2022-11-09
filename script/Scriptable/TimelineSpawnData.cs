using System;
using System.Collections.Generic;
using script.Spawn;
using UnityEngine;

namespace script.Scriptable
{
    [Serializable]
    public struct TimelineSpawnTask
    {
        
        public float spawnTime;
       
        /// spawn位置相对于spawnManager原点位置的偏移量
        public Vector3 startPositionOffset;
      
        /// 连续物体的间隔offset
        public Vector3 flowOffset;
        
        ///多少个物体
        public int count;
        /// 多个物体之间的间隔
        public float interval;
        public EnemyData setting;
    }
    
    [CreateAssetMenu(fileName = "TimelineSpawnData", menuName = "data/TimelineSpawnData", order = 0)]
    public class TimelineSpawnData : EnemySpawnData
    {
        public List< TimelineSpawnTask> tasks;

        private void OnEnable()
        {
            tasks?.Sort((a, b) => a.spawnTime<b.spawnTime?-1:1);
        }
    }
}