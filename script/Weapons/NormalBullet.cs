using System;
using script.Buffer;
using script.consts;
using script.DamageCalculate;
using UnityEngine;

namespace script.Weapons
{
    public class NormalBullet:Bullet
    {
        public NormalBullet()
        {
            MTranslateUpdate = () =>
            {
                MTransform.Translate ( Vector3.forward * (speed.GetResultValF() * Time.deltaTime));//直直地往前走
            };
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Spaceship.Spaceship>(out var sp)) return;

            if (!other.TryGetComponent<IDamageRecipient>(out var damageRecipient)) return;
            // Debug.Log($"hit {sp.entityType}");

            if (sp.entityType.IsOppositeEntity(ammoOwner))
            {
                Debug.Log($"normal bullet hit {sp.entityType} {sp.name} {sp.tag}");
                HitOtherEntity(damageRecipient, GiveDamage());//doing damage calculate
            }
        }
        public override void HitOtherEntity(IDamageRecipient damageReceiver,  int damage)
        {
            var afterStat = damageReceiver.BeingDamaged(damage);
            ManualInvokeAfterDamageCallback(afterStat);
            //disappear TODO change to effect
            Destroy(gameObject);
        }
        public override int GiveDamage()
        {
            return (int) damage.ResultVal;
        }
        
       
       

       
    }
}