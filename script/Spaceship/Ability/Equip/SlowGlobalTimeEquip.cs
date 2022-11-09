using System.Collections;
using script.Scene;
using UnityEngine;

namespace script.Spaceship.Ability.Equip
{
    public class SlowGlobalTimeEquip : SpaceshipAbilityEquip
    {
        public override AbilityIntro GetIntro()
        {
            return new AbilityIntro
            {
                ChineseName = "时空停滞",
                ChineseIntro = "当你收到攻击时，使周围时间变得缓慢（10s冷却时间）",
                SpritePath = "Images/TimeSlowIcon",

            };
        }


        public float slowFactor =>ProjectVariables.Instance.slowTimeFactor;
        public float coolDownTime = 10f, 
            // lastProcessBeginTime = 0f,
            lastProcessEndTime=0f;

        public float processSustainTime = 1.5f;
        // public float las = 0f;

        public bool isInProcess;
        public override void Do(Spaceship spaceship)
        {
            
            Debug.Log("spaceship.OnBeingDamaged += TryBeginProcess;");
            spaceship.OnBeingDamaged += TryBeginProcess;
        }

        private void TryBeginProcess(int d)
        {
            if (isInProcess||d<=0)
            {
                return;
            }

            if (lastProcessEndTime > 0 && Time.time - lastProcessEndTime < coolDownTime)
            {
                return;
            }

            //begin process
            BeginProcess(d);

        }

        private void BeginProcess(int d)
        {
            isInProcess = true;
            // lastProcessBeginTime = Time.time;
            Time.timeScale = slowFactor;
            // Debug.Log($"Set timescale {Time.timeScale}");
            //定时结束
            GlobalPlayerManger.Instance.Player.StartCoroutine(EndProcess());
        }

        private IEnumerator EndProcess()
        {
            yield return new WaitForSeconds(processSustainTime);
            isInProcess = false;
            lastProcessEndTime = Time.time;
            Time.timeScale = 1f;
        }
        public override void Remove(Spaceship spaceship)
        {
            spaceship.OnBeingDamaged -= TryBeginProcess;
        }
    }
}