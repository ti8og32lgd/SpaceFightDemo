using System;
using System.Collections.Generic;
using script.Scriptable;
using script.Spawn;
using script.UI;
using UnityEngine;

namespace script
{
    /// <summary>
    /// 公共变量
    /// </summary>
    public class ProjectVariables : Singleton<ProjectVariables>
    {
        [Header("string")]
        public string fightCanvasName= "Fight Canvas";
        public string fightSceneName="fight";
        public string fightPlaneName="FightPlane";

        [Header("prefab")] 
        public  GameObject normalWeapon;
        public GameObject bombWeapon;

        // public GameObject abilityIntro;
        public GameObject abilityUIItemPrefab;
        public  AudioClip defaultFireAudioClip;
        public HPBar enemyHpBarPrefab;

        public Transform defaultExplosionFx;
        [Header("Material")] 
        public Material goldenShieldMaterial;
        [Header("level data")] 
        public List<LevelSpawnSetting> LevelEnemySpawnSettings;
        public List<PropSpawnData> LevelPropSpawnDatas;
        [Header("Map setting")] 
        /// 战斗区域平面向上方向
        public Vector3 fightPlaneForward;
        /// 战斗区域中心
        public Vector3 fightPlaneCenter;
        /// 战斗区域平面法线
        public Vector3 fightPlaneNormal;

        public Vector3 FightPlaneHorizontal=>Vector3.Cross(fightPlaneForward,fightPlaneNormal).normalized;
        public float fightPlaneWidth = 1f;
        
        [Header("Health")]
        public int recycleHealValue;
        [Header("Ability setting")]
        public float slowTimeFactor=0.5f;

        
        [Header("some float")]
        public float threshold=0.1f;

        public float wingmanHpFactor=0.5f;
        public float wingmanAttackFactor=0.5f;
        public GameObject wingmanPrefab;
        public float generalShieldDamageCoolDown=2f;
        public float besselFactor=20f;

        [Header("for debug and test")]
        public PropData testPropData;

    }

    [Serializable]
    public struct SpawnSetting
    {
        public EnemySpawnManager spawnManager;
        public EnemySpawnData data;
    }
    
    [Serializable]
    public struct LevelSpawnSetting
    {
        public List<SpawnSetting> settings;
    }
}