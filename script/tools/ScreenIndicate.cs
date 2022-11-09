//直接在画面显示一些东西，测试用

using System.Collections;
using UnityEngine;

namespace script.tools
{
    public class ScreenIndicate:Singleton<ScreenIndicate>
    {
        [SerializeField] private GameObject indicatorPrefab;
        [SerializeField] private float disappearTime;
        public  void IndicateWorldPos(Vector3 wordPos)
        {
            var gameObj= Instantiate(indicatorPrefab);
            gameObj.transform.position = wordPos;
            DestroyIndicator(gameObj, disappearTime);
        }

        void DestroyIndicator(GameObject indicator,float indicateTime)
        {
            Destroy(indicator,indicateTime);
        }
    }
}