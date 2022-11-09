using System;
using script.EnemyInstance;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.Scriptable
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [Header("module")] 
        public string spaceshipModulePath = "Module/?";
        // public string weaponModulePath = "Module/Weapon/?";
        // public string bulletModulePath = "Module/Weapon/?";
        [Header("attack & move")] 
        public EnemyFactory.MoveType moveType;
        public EnemyFactory.AttackType attackType;
        public EnemyFactory.WeaponType weaponType;
        public bool isFindPlayerAsTarget;

        [Header("speed value")] 
        public float moveSpeed,rotateSpeed,bulletSpeed;
        [Header("fire value")]
        public float fireCoolDown = 1.1f;
        public float firstFireTime {
            get
            {
                var o=((float)RandNumber+0.1f) % fireCoolDown;//首次开火时间   
                // Debug.Log(o);
                return o;
            }
        }
        [Header("hp value")]
        public int maxHp=300,nowHp=300;

        [Header("damage value")] 
        public int baseDamge = 25;
        public int shellDamge = 25;
        
        [Header("voice")] public float volume = 0.1f;
        public string shootAudioClipPath = "Audio/Shoot/?";
        
        [Header("Rand")] public int randSeed = 5;

        public int RandNumber
        {
            get
            {
                // Random.InitState(randSeed);
                
                return  Random.Range(0, Int32.MaxValue);
            }
        }

    }
}