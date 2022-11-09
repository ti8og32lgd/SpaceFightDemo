using System;
using script.EnemyInstance;
using script.Scriptable;
using UnityEngine;

namespace script.ProjectDebug
{
    public class EnemyFactoryTest : MonoBehaviour
    {
        public EnemyData[] datas;
        
        public void Start()
        {
            T1();
        }

        private void T1()
        {
            float  i = 0;
            foreach (var d in datas)
            {
               
                var obj=EnemyFactory.CreateEnemy(d);
                obj.transform.SetParent(transform);
                obj.transform.localRotation=Quaternion.Euler(0,0,0);
                // obj.transform.Roa(0,0,0,Space.Self);
                obj.transform.localPosition = new Vector3(0.1f * i, 0, 0);
               
            }
        }
    }
}