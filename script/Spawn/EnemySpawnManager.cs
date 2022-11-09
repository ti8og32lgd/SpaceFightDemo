using System;
using script.Scriptable;
using UnityEngine;

namespace script.Spawn
{
    public abstract class EnemySpawnManager : MonoBehaviour
    {
        public EnemySpawnData spawnData;
        public abstract void BeginSpawn();

        public event Action OnSpawnDone;

        public Transform fightingPlane
        {
            get
            {
                var obj=GameObject.Find(ProjectVariables.Instance.fightPlaneName);
                return obj.transform;
            }
        }
        public void InvokeSpawnDone()
        {
            OnSpawnDone?.Invoke();
        }
    }
}