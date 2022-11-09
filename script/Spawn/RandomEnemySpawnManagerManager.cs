using System.Collections;
using System.Collections.Generic;
using script.EnemyInstance;
using script.Scriptable;
using script.Spaceship;
using script.Spaceship.Prop;
using UnityEngine;

namespace script.Spawn
{
    
    public class RandomEnemySpawnManagerManager : EnemySpawnManager
    {

        // public Transform spawnParentTransform;
        public RandomEnemySpawnData randomSpawnData
        {
            get
            {
                return (RandomEnemySpawnData) spawnData;
            }
            set
            {
                sumWeight =CalculateSumWeight(value);
                spawnData = value;
            }
        }


        
        public bool isSpawning;

        [Header("Statistics")]
        public float sumWeight;
        public int totalEnemyCount;
        public int destroyedEnemyCount;

        
        private float spawnTime = 0f;
        private int spawnRound = 0;
        
        public Vector3 SpawnPosition
        {
            get
            {
                var offset=ProjectVariables.Instance.FightPlaneHorizontal*ProjectVariables.Instance.fightPlaneWidth*Random.Range(-1f,1f);
                // Debug.Log("spawn prop offset "+offset);
                return transform.position+offset;
            }  
        }
        
        
     

        private void Start()
        {
            // transform.position = spawnData.initSpawnPosition;
            isSpawning = true;
        }
    
       
        public override void BeginSpawn()
        {
            sumWeight =CalculateSumWeight(randomSpawnData);
            isSpawning = true;
            StartCoroutine(SpawnProcess());

        }
        
        private IEnumerator SpawnProcess()
        {
            while (isSpawning)
            {
                
                float nextSpawnTime = Random.Range(randomSpawnData.intervalMin, randomSpawnData.intervalMax);
                yield return new WaitForSeconds(nextSpawnTime);
                int count = Random.Range(randomSpawnData.countOnceMin, randomSpawnData.countOnceMax);
                var enemyData = RandomSelectEnemyData();

                for (int i = 0; i < count; i++)
                {
                    var obj = EnemyFactory.CreateEnemy(enemyData);
                    obj.GetComponent<Enemy>().OnSpaceshipDestroy += SpawnEnemyDestroyCallback;
                    obj.transform.forward = -ProjectVariables.Instance.fightPlaneForward;
                    obj.transform.position = SpawnPosition;
                    obj.transform.SetParent(fightingPlane);

                    totalEnemyCount++;
                    yield return new WaitForSeconds(randomSpawnData.minSpawnInterval);
                }

                spawnRound++;
                spawnTime += nextSpawnTime + randomSpawnData.minSpawnInterval;
                if (spawnRound >= randomSpawnData.spawnEndRound || spawnTime > randomSpawnData.spawnEndTime)
                {
                    isSpawning = false;
                }

            }

        }

        
        /// <summary>
        ///notify one enemy destroyed;
        /// </summary>
        private void SpawnEnemyDestroyCallback()
        {
            destroyedEnemyCount++;
            if (!isSpawning && destroyedEnemyCount == totalEnemyCount)
            {
                InvokeSpawnDone();
            }
        }
        private EnemyData RandomSelectEnemyData()
        {
            var randomNumber = Random.Range(0, sumWeight);

            foreach (var item in randomSpawnData.enemyWeightList)
            {
                randomNumber -= item.weight;
                if (randomNumber < 0f)
                {
                    Debug.Log("Enemy Random: "+randomNumber+" / "+sumWeight+" => "+item.data.name);

                    return item.data;
                }
            }
            return null;
        }

     
        
        private float CalculateSumWeight(RandomEnemySpawnData value)
        {
            var sum = 0f;
            foreach (var i in value.enemyWeightList)
            {
                sum += i.weight;
            }

            return sum;
        }
    }
}