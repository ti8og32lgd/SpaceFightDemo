using System.Collections;
using script.Buffer;
using script.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace script.Spaceship.Ability.Equip
{
    public class WingmanEquip : SpaceshipAbilityEquip
    {

        public GameObject wingmanPrefab=>ProjectVariables.Instance.wingmanPrefab;
        public override AbilityIntro GetIntro()
        {
            return new AbilityIntro
            {
                ChineseName = "小帮手",
                ChineseIntro = "生成一个僚机，可以攻击敌人和抵挡伤害",
                SpritePath = "Images/WingmanIcon 1"

            };
        }


        public float attackFactor =ProjectVariables.Instance.wingmanAttackFactor,
            hpFactor=ProjectVariables.Instance.wingmanHpFactor
            ;

        public Spaceship host;
        public override void Do(Spaceship spaceship)
        {
            host = spaceship;
            if (SceneManager.GetActiveScene().name == ProjectVariables.Instance.fightSceneName)
            {
                InitWingman();
            }
            SceneManager.sceneLoaded += BindSceneLoad;
        }

        private void BindSceneLoad(UnityEngine.SceneManagement.Scene scene, LoadSceneMode arg1)
        {
            if (scene.name == ProjectVariables.Instance.fightSceneName)
            {
                host.StartCoroutine(LagInitWingman());
            }
        }

        private IEnumerator LagInitWingman()
        {
            yield return new WaitForSeconds(1);
            InitWingman();
            yield return null;
        }


        private void InitWingman()
        {
            Debug.Log("Init wingman");
            var wingmanObj=MonoBehaviour.Instantiate(wingmanPrefab);
            wingmanObj.GetComponent<Wingman>().Init();
            wingmanObj.GetComponent<Wingman>().baseDamage = host.baseDamage.Clone();
            wingmanObj.GetComponent<Wingman>().baseDamage.AddBufferUnit(new BufferUnit("wingman discount ","",BufferUnit.BufferType.Mul,(decimal)attackFactor));
            wingmanObj.GetComponent<Wingman>().maxHp = (int)(host.maxHp * hpFactor);
            wingmanObj.GetComponent<Wingman>().nowHp = (int)(host.maxHp * hpFactor);
            
        }
        public override void Remove(Spaceship spaceship)
        {
            SceneManager.sceneLoaded -= BindSceneLoad;
        }
    }
}