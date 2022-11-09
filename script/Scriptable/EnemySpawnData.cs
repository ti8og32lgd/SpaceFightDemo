using UnityEngine;

namespace script.Scriptable
{
    public class EnemySpawnData : ScriptableObject
    {
        public Vector3 initSpawnPosition;
        public float minSpawnInterval = 0.1f;
    }
}