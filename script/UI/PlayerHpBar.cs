using System;
using TMPro;
using UnityEngine;

namespace script.UI
{
    [DefaultExecutionOrder(2)]
    public class PlayerHpBar : MonoBehaviour
    {
        public HPBar bar;
        public TextMeshProUGUI nowHpTxt, fullHpTxt;
        public Spaceship.Spaceship sp;


        public void Init()
        {
            sp = GetComponent<Spaceship.Spaceship>();
            bar.SetNowHp(sp.nowHp);
            bar.nowDisplayHp = sp.nowHp;
            bar.SetFullHpAndRefreshBar(sp.maxHp);
            fullHpTxt.text = sp.maxHp.ToString();
            nowHpTxt.text = sp.nowHp.ToString();
            
            sp.OnLifeChanged += spOnOnLifeChanged;
            Debug.Log("PlayerHpBar Init done");
        }
        public void spOnOnLifeChanged(int nowHp)
        {
            Debug.Log($"spOnOnLifeChanged begin {bar} {nowHp} {sp}");
            bar.SetNowHp(nowHp);
            nowHpTxt.text = nowHp.ToString();
            fullHpTxt.text = sp.maxHp.ToString();
        }

        private void OnDestroy()
        {
            sp.OnLifeChanged -= spOnOnLifeChanged;
        }
    }
}