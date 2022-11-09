using System;
using System.Collections;
using System.Collections.Generic;
using script.consts;
using script.DamageCalculate;
using UnityEngine;

namespace script.Weapons
{
    public class LaserBullet : Bullet
    {
        public  List<IDamageRecipient> damageRecipients=new ();
        public float damageCalculateInterval = 1.0f;
        public void OnEnable()
        {

            StartCoroutine(PeriodicallyCalculateDamage());
            // m_transform = transform;  //not use
        }
        
        public void Start()
        {
            //over write
        }

        public void Update()
        {
            //over write
        }

        private void OnTriggerEnter(Collider other)
        {
            //每秒结算一次伤害
            if (!other.CompareTag(ammoOwner.GetEntityTag()))//cause damage;
            {
                if (other.TryGetComponent<IDamageRecipient>(out var damageReceiver))
                {
                    // Debug.Log($"laser shoot {other.name}");
                    damageRecipients.Add(damageReceiver);
                    // HitOtherEntity(damageReceiver, GiveDamage());
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(ammoOwner.GetEntityTag()))//cause damage;
            {
                if (other.TryGetComponent<IDamageRecipient>(out var damageReceiver))
                {
                    // Debug.Log($"laser escape {other.name}");
                    bool ok=damageRecipients.Remove(damageReceiver);
                    if (!ok)
                    {
                        Debug.Log(" damageRecipients remove  not suc");
                    }
                }
            }
        }


        IEnumerator PeriodicallyCalculateDamage()
        {
            while (true)
            {
                yield return new WaitForSeconds(damageCalculateInterval);

                foreach (var recipient in damageRecipients)
                {
                    HitOtherEntity(recipient, GiveDamage());
                }
            }
        }
        
        
        public override void HitOtherEntity(IDamageRecipient damageReceiver,  int damage)
        {
            var afterStat = damageReceiver.BeingDamaged(damage);
            if (afterStat.IsDead == true)
            {
                bool ok=damageRecipients.Remove(damageReceiver);
                if (!ok)
                {
                    Debug.Log(" HitOtherEntity remove  not suc");
                    
                }
            }
            ManualInvokeAfterDamageCallback(afterStat);
        }
        
    }
}