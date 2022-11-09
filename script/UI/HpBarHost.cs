using System;
using UnityEngine;

namespace script.UI
{
    public class HpBarHost:MonoBehaviour
    {
        public HPBar prefab => ProjectVariables.Instance.enemyHpBarPrefab;
        public RectTransform displayCanvas;
        private Transform mFollow;
        private HPBar mBar;

        private string fightCanvasName => ProjectVariables.Instance.fightCanvasName;
        public Vector3 offset = new Vector3(0, 0, 0.5f);
        private Transform follow
        {
            get
            {
                if (mFollow == null)
                {
                    mFollow = GetComponent<Transform>();
                }

                return mFollow;
            }
        }


        public HPBar bar
        {
            get
            {
                if (mBar == null)
                {
                    mBar = Instantiate(prefab,canvas);
                    var sp=follow.GetComponent<Spaceship.Spaceship>();
                    if (sp == null)
                    {
                        Debug.LogError("follow not a spaceship");
                    }
                    mBar.SetNowHp(sp.nowHp);
                    mBar.nowDisplayHp = sp.nowHp;
                    mBar.SetFullHpAndRefreshBar(sp.maxHp);

                    sp.OnLifeChanged += (hp) =>
                    {
                        
                        mBar.SetNowHp(hp);
                        if (hp == 0)
                        {
                            Destroy(mBar.gameObject);
                            // Destroy(gameObject);
                        }

                    };
                }

                return mBar;
            }
        }


        private RectTransform canvas
        {
            get
            {
                if (displayCanvas == null)
                {
                    displayCanvas=GameObject.Find(fightCanvasName).GetComponent<RectTransform>();
                }

                return displayCanvas;
            }
        }
        private void Update()
        {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, follow.position + offset);
           
            if (
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, null,
                    out var localPoint)
            )
            {
                // Debug.Log(localPoint);
                // obj.GetComponent<RectTransform>().anchoredPosition = localPoint;
                bar.GetComponent<RectTransform>().localPosition = localPoint;
            }
        }

        private void OnDestroy()
        {
            Destroy(bar.gameObject);
        }
    }
}