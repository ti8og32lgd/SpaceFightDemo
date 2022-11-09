using script.DamageCalculate;
using script.Spaceship.Part;
using UnityEngine;

namespace script.Spaceship.Ability.Equip
{
    public class RecycleHealEquip : SpaceshipAbilityEquip
    {
        private int healValue => ProjectVariables.Instance.recycleHealValue;
        public override AbilityIntro GetIntro()
        {
            return new AbilityIntro
            {
                ChineseName = "外太空回收",
                ChineseIntro = "击落敌机时，回复一定生命值",
                SpritePath = "Images/RecycleIcon",
            };
        }

        private Spaceship host;
        public override void Do(Spaceship spaceship)
        {
            host = spaceship;
            spaceship.OnCauseDamage +=HealProcess;
            return;
       
        }

        private void HealProcess(AfterDamageStatus stat)
        {
            if (stat.IsDead != true && stat.RealDamage>0)
            {
                return;
            }

            host.BeingHealed(healValue);
        }
        
        
        public override void Remove(Spaceship spaceship)
        {
            host = null;
            spaceship.OnCauseDamage -=HealProcess;
        }
    }
}