using System;

namespace script.Spaceship.Ability.Equip
{

    public struct AbilityIntro
    {
        public string ChineseName,ChineseIntro,SpritePath;

        public AbilityIntro(string name, string intro,string spritePath)
        {
            ChineseIntro = intro;
            ChineseName = name;
            SpritePath = spritePath;
        }
    }
    public class SpaceshipAbilityEquip
    {
        ///冷却时间更新的callback
        /// 参数：冷却时间准备完成度 range(0,1) 
        public event Action<float> OnCoolDownCallback;

        public void InvokeOnCoolDown(float f)
        {
            OnCoolDownCallback?.Invoke(f);
        }

        public void ClearOnCoolDown()
        {
            OnCoolDownCallback = null;
        }
        public virtual AbilityIntro GetIntro()
        {
            return new AbilityIntro();
        }
        
        
        public virtual void Do(script.Spaceship.Spaceship spaceship)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Remove(Spaceship spaceship)
        {
            throw new System.NotImplementedException();
        }
    }
}