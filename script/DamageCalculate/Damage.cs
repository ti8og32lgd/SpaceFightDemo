using System;

/*
   * damage calculate process:
   * 1. retrieve damage : int GiveDamage()
   * 2. notify recipient: AfterDamageStatus beingDamaged(int damage)
   * 3. notify giver: callback void AfterDamage(AfterDamageStatus stat) 
   */
namespace script.DamageCalculate
{
    public struct AfterDamageStatus
    {
        public bool IsDead { get; set; }
        public int RealDamage { get; set; }
    }

    public interface IDamageRecipient //recipient
    {
        /// <summary>
        /// 收到伤害
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public  AfterDamageStatus BeingDamaged(int damage);
    }

    public interface IDamageGiver
    {
        /// <summary>
        /// 对其他造成伤害后获取反馈
        /// </summary>
        /// <param name="status"></param>
        public void DamagedCallback(AfterDamageStatus status);
        // public event Action<AfterDamageStatus> AfterDamageCallback;
        // public int GiveDamage();
    }
}