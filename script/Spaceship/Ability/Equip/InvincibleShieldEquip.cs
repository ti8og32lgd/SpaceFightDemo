using script.DamageCalculate;
using script.Spaceship.Ability.Component;
using UnityEngine;

namespace script.Spaceship.Ability.Equip
{
    public class InvincibleShieldEquip:SpaceshipAbilityEquip
    {
        private AbilityIntro intro=new( "无敌护盾","收到伤害时获取一段时间的无敌护盾","Images/shield_yellow");
        public override AbilityIntro GetIntro()
        {
            return intro;
        }
        
        private InvincibleShield invincibleShield;
        public override void Do(Spaceship spaceship)
        {
            if(spaceship.shield==null) spaceship.CreateShield();
            invincibleShield=spaceship.shield.gameObject.AddComponent<InvincibleShield>();
            spaceship.OnLifeChanged += InvincibleEvent;
            invincibleShield.OnCoolDownCallback += base.InvokeOnCoolDown;
        }

        private  void InvincibleEvent(int hp)
        {
            invincibleShield.TryEnableInvincible();
        }
        
        
        public override void Remove(Spaceship spaceship)
        {
            if (spaceship.shield != null && spaceship.shield.GetComponent<InvincibleShield>() != null)
            {
                MonoBehaviour.Destroy(spaceship.shield.GetComponent<InvincibleShield>());
            }
            spaceship.OnLifeChanged -= InvincibleEvent;
        }
    }
}