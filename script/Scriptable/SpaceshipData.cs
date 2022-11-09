using System;
using script.EnemyInstance;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.Scriptable
{
    [CreateAssetMenu(fileName = "SpaceshipData", menuName = "data/SpaceshipData", order = 0)]
    public class SpaceshipData : ScriptableObject
    {
        [Header("speed value")] 
        public float moveSpeed,rotateSpeed,bulletSpeed;
        [Header("fire value")]
        public float fireCoolDown = 1.1f;
        public float firstFireTime {
            get
            {
                var o=((float)RandNumber) % fireCoolDown+0.1f;//首次开火时间   
                return o;
            }
        }
        [Header("hp value")]
        public int maxHp=300,nowHp=300;

        [Header("damage value")] 
        public int baseDamage = 25;
        public int shieldDamage = 25;
        
        
        [Header("Rand")] public int randSeed = 5;

        public int RandNumber
        {
            get
            {
                return  Random.Range(0, Int32.MaxValue);
            }
        }

        public void OnEnable()
        {
            // Random.InitState(randSeed);
        }
    }
}