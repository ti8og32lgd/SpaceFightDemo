using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace script.UI
{
    public class HPBar : MonoBehaviour
    {
        [Header("hp attributes")] 
        public int fullHp = 100;//满生命值
        public int nowHp = 50;//当前生命值
        public int nowDisplayHp = 60;//当前动画显示到的生命值
        public int perUnitHp = 25;//每一个格子表示的血量
        [Header("display setting")]
        public Color unitColor = Color.green;
        public Color loseHpEffectColor=Color.red;//丢失生命值时的特效颜色
        public float hopInterval = 0.1f;//每次刷新血条的间隔
        private float timeSinceHop = 0f;
        //血量单元距离血条边框的纵向距离
        [SerializeField]  private float unitMargin = 0.1f;

        [Header("component binding")] 
        public RectTransform barBackground;//背景板
        public RectTransform  barFill;//填充unit的板子
        public GameObject hpUnitPrefab;//单位血量的prefab
        
        //HP unit def
        
        //血条总宽度
        private float hpPanelWidth;
        private float HpPanelWidth
        {
            get
            {
                if (hpPanelWidth == 0)
                {
                    hpPanelWidth=barFill.rect.width;
                }

                return hpPanelWidth;
            }
        }

        //血量单元数量
        private int HpUnitCount => fullHp / perUnitHp;
        //每个血量单元的宽度
        private float PerUnitWidth => (HpPanelWidth-unitMargin) / (HpUnitCount+unitMargin);
        //血量单元的位置
        private List<Vector3> hpUnitPositions;
        //血量单元的初始高度
        private float HpUnitHeight => barFill.rect.height;
       

        //血量单元GameObjectList
        private List<GameObject> mHpUnits;
        private List<GameObject> HpUnits
        {
            get
            {
                if (mHpUnits == null)
                {
                    GenerateHpUnits();
                }

                return mHpUnits;
            }
        }

        private void GenerateHpUnits()
        {
            if (mHpUnits != null)
            {
                foreach (var obj in mHpUnits)
                {
                    Destroy(obj);
                }
            }

            if (perUnitHp ==0)
            {
                Debug.Log($"{perUnitHp} {fullHp} {nowHp} {nowDisplayHp}");
            }
            mHpUnits = new List<GameObject>(HpUnitCount);
            hpUnitPositions = new List<Vector3>(HpUnitCount);
            // hpUnitHeight = new List<float>(HpUnitCount);

            var nowIndex = GetHpUnitIndex(nowHp, perUnitHp);
            var startX = -HpPanelWidth / 2;
            for (var i = 0; i < mHpUnits.Capacity; i++)
            {
                //initiate
                var obj = Instantiate(hpUnitPrefab, transform);
                var positionX = startX+ i * (PerUnitWidth+unitMargin) + 0.5f * PerUnitWidth;
                // Debug.Log($"{i} {positionX} {PerUnitWidth}");
                var pos = new Vector3(positionX, 0, 0);


                obj.GetComponent<HpUnit>().SetPositionAndSize(pos, HpUnitHeight, PerUnitWidth);
                obj.GetComponent<HpUnit>().SetColor(unitColor);

                hpUnitPositions.Add(pos);

                mHpUnits.Add(obj);
                if (i > nowIndex)
                {
                    obj.SetActive(false);
                }
            }
        }

        private void Start()
        {
            //init
            var x = HpUnits;
        }

       

        private void Update()
        {
            RefreshHpBar();
        }

    
        //HP unit end
        public void SetNowHp(int nowHp)
        {
            this.nowHp = nowHp;
        }
        
        public void SetFullHpAndRefreshBar(int hp)
        {
            fullHp = hp;
            // nowHp = hp;
            // nowDisplayHp = hp;
            GenerateHpUnits();
        }

        public void RefreshHpBar()
        {
            timeSinceHop -= Time.deltaTime;
            if (timeSinceHop > 0f || nowDisplayHp>fullHp)
            {
                return;
            }
            timeSinceHop = hopInterval;//reset timer
            
            if (nowHp == nowDisplayHp)//no action
            {
                return;
        
            }else if (nowHp > nowDisplayHp)
            {
                //restore units
                int nowIndex = GetHpUnitIndex(nowDisplayHp,perUnitHp);//当前最后显示的unit index
                int lastIndex =GetHpUnitIndex(nowHp,perUnitHp);//当前血量应该显示的位置
                for (var i = nowIndex + 1; i <= lastIndex; i++)
                {
                    HpUnits[i].SetActive(true);
                    HpUnits[i].GetComponent<HpUnit>().ResetRect();
                }
                nowDisplayHp = nowHp;//reset value
                var fillAmount = (nowHp % perUnitHp == 0) ? perUnitHp : nowHp % perUnitHp;
                HpUnits[lastIndex].GetComponent<HpUnit>().SetFill((float)fillAmount/perUnitHp);
            }else if (nowHp < nowDisplayHp)
            {
                
                if (GetHpUnitIndex(nowDisplayHp,perUnitHp) == GetHpUnitIndex(nowHp,perUnitHp))                //在一个perUnitHp内： hp unit填充进行滑动
                {
                    int index = GetHpUnitIndex(nowDisplayHp, perUnitHp);
                    HpUnits[index].GetComponent<HpUnit>().SetFill((float)(nowHp%perUnitHp)/perUnitHp);
                    nowDisplayHp = nowHp;
                }
                else                //差大于perUnitHp：当前 hp unit进行摧毁动画
                {
                    int index = GetHpUnitIndex(nowDisplayHp, perUnitHp);
                    if (index >= HpUnits.Count)
                    {
                        Debug.Log($" {fullHp} {nowHp} {nowDisplayHp} {index} {HpUnits.Count}");
                        return;
                    }
                    HpUnits[index].GetComponent<HpUnit>().DisplayLose();
              
                    nowDisplayHp -= perUnitHp;
                }
            }
        }

        private int GetHpUnitIndex(int hp, int perUnitHp)
        {
            if (hp == 0) return 0;
            return hp / perUnitHp - (hp%perUnitHp==0?1:0);
        }
    }
}