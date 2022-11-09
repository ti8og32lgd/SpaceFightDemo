using script.Spaceship.Prop;
using UnityEngine;

namespace script.Scriptable
{
    [CreateAssetMenu(fileName = "PropData", menuName = "data/PropData", order = 0)]
    public class PropData : ScriptableObject
    {
        /// <summary>
        /// 显示物体的prefab
        /// </summary>
        public GameObject ObjPrefab;


        public PropGenerator.PropType type;
        
        /// <summary>
        ///数值
        /// </summary>
        public float value;

        public bool isObjRotate;
    }
}