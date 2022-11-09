using System;
using System.Collections;
using System.Linq;
using script.Scriptable;
using script.Spaceship;
using script.Spaceship.Prop;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.Spawn
{
    /// <summary>
    ///两种生成方式：
    /// 1. 飞到战斗区域
    /// 2. enemy死亡后出来
    /// </summary>
    public class PropSpawnManger : MonoBehaviour
    {

        private PropSpawnData propSpawnData;
        public PropSpawnData PropSpawnData
        {
            get => propSpawnData;
            set
            {
                sumWeight =CalculateSumWeight(value);
                propSpawnData = value;
            }
        }

     

        public float propSpawnIntervalMin = 10f;
        public float  propSpawnIntervalMax = 20f;

        public bool isSpawning;

        public float sumWeight;

        public Vector3 SpawnPosition
        {
            get
            {
                var offset=ProjectVariables.Instance.FightPlaneHorizontal*ProjectVariables.Instance.fightPlaneWidth*Random.Range(-1f,1f);
                Debug.Log("spawn prop offset "+offset);
                return transform.position+offset;
            }  
        }
        
        public PropSpawnManger(PropSpawnData data)
        {
           PropSpawnData = data;
        }

        public PropSpawnManger()
        {
            isSpawning = true;
        }

        public void BeginSpawn()
        {
            StartCoroutine(SpawnProcess());
        }
        
        private IEnumerator SpawnProcess()
        {
            while (isSpawning)
            {
                float nextSpawnTime = Random.Range(propSpawnIntervalMin, propSpawnIntervalMax);
                yield return new WaitForSeconds(nextSpawnTime);

                var propData = RandomSelectPropData();
                
                int type = Random.Range(0, 100) % 2; //1 or 0
                if (type == 0) {
                    var enemy = FindOneEnemyCanContainProp();
                    if (enemy == null||enemy.OnEnemyDead!=null)
                    {
                        type = 1;
                    }
                    else
                    {
                        enemy.OnEnemyDead += (em) =>
                        {
                            var propObj = PropGenerator.CreatePropGameObject(propData);
                            propObj.transform.position = em.transform.position;
                        };
                    }
                }
                
                
                if (type == 1)
                {
                    var propObj = PropGenerator.CreatePropGameObject(propData);
                    propObj.transform.position = SpawnPosition;
                }
                
               
                
            }

        }
        
        
        
        private Enemy FindOneEnemyCanContainProp()
        {
            var e= FindObjectOfType<Enemy>();
            return e;
        }
        private PropData RandomSelectPropData()
        {
            var randomNumber = Random.Range(0, sumWeight);
            foreach (var item in PropSpawnData.PropList)
            {
                randomNumber -= item.weight;
                if (randomNumber < 0f)
                {
                    Debug.Log("Prop Random: "+randomNumber+" / "+sumWeight+" => "+item.data.type);

                    return item.data;
                }
            }
            return null;
        }

     
        
        private float CalculateSumWeight(PropSpawnData value)
        {
            var sum = 0f;
            foreach (var i in value.PropList)
            {
                sum += i.weight;
            }

            return sum;
        }
        
    }
}