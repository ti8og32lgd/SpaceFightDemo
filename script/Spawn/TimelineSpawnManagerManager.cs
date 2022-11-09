using System;
using System.Collections;
using System.Collections.Generic;
using script.EnemyInstance;
using script.Scriptable;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace script.Spawn
{
    public class TimelineSpawnManagerManager:EnemySpawnManager
    {
        public TimelineSpawnData data;
        public Transform spawnParentTransform;
      
        private int trackIndex;
        private float spawnTime;
        public void  InitTimelineSpawnManager(TimelineSpawnData data,Transform spawnParentTransform)
        {
            this.data = data;
            trackIndex = 0;
            spawnTime = 0f;
            this.spawnParentTransform = spawnParentTransform;
        }

        public override void BeginSpawn()
        {
            throw new NotImplementedException();
        }
        

        /// <summary>
        /// timeline执行完毕调用
        /// </summary>
        public event Action OnTimelineDone;

        private bool isFlowDone;
        private void Update()
        {
            if (data == null) return;
            
            if (trackIndex >= data.tasks.Count)
            {
                if (!isFlowDone) return;
                OnTimelineDone?.Invoke();
                Destroy(this);
                return;
            }
            spawnTime += Time.deltaTime;
            
            if (spawnTime >= data.tasks[trackIndex].spawnTime)//exceed time
            {
                var positions = GetFlowPositions( data.tasks[trackIndex].startPositionOffset ,data.tasks[trackIndex].flowOffset, data.tasks[trackIndex].count);
                StartCoroutine(SpawnFlow(positions, data.tasks[trackIndex].setting, data.tasks[trackIndex].interval));
                trackIndex++;
            }
        }

        private Vector3[] GetFlowPositions( Vector3 startPosition,Vector3 offset, int count)
        {
            Vector3[] positions = new Vector3[count];
            for (var i = 0; i < count; i++)
            {
                Vector3 pos=startPosition+offset*i;
                positions[i] = pos;
            }
            
            return positions;
        }

        public IEnumerator SpawnFlow(Vector3[] positions,EnemyData  enemyData,float interval)
        {
            isFlowDone = false;
            foreach (var pos in positions)
            {

                var obj = EnemyFactory.CreateEnemy(enemyData);
                obj.transform.SetParent(spawnParentTransform);
                obj.transform.localRotation=Quaternion.Euler(0,0,0);
                obj.transform.localPosition = pos;
                
                yield return new WaitForSeconds(interval);
            }
            isFlowDone = true;

        }


        
    }
}