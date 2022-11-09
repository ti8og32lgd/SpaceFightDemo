
using System;
using UnityEngine;
namespace script
{
    public class Singleton<T>:MonoBehaviour where T:Component
    {
        public static T Instance { get; private set; }


        public virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this as T ;
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            if(Instance==this)
                Instance = null;
        }
        
    }
}