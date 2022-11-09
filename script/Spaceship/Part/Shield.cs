using System;
using System.Collections.Generic;
using System.Linq;
using script.consts;
using script.DamageCalculate;
using UnityEngine;
using UnityEngine.Animations;

namespace script.Spaceship.Part
{
    public class Shield : MonoBehaviour
    {
        public Spaceship mOwner;

        public Spaceship owner
        {
            get
            {
                if (mOwner == null)
                {
                    mOwner = GetComponentInParent<script.Spaceship.Spaceship>();
                }

                return mOwner;
            }
            set => mOwner = value;
        }

        public float damageCoolDown => ProjectVariables.Instance.generalShieldDamageCoolDown;
        private Dictionary<IDamageRecipient,float>  coolDownTable=new();
        private void Update()
        {
            // Debug.Log(coolDownTable.Count);
            foreach (var key in coolDownTable.Keys.ToList())
            {
                coolDownTable[key] -= Time.deltaTime;
                if ( coolDownTable[key]  < 0)
                {
                    coolDownTable.Remove(key);
                }
            }
            
        }

        public bool isDestroyBullet;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(owner.tag))//cause damage;
            {
                if (isDestroyBullet && other.TryGetComponent<Bullet>(out var hitBullet) && hitBullet.ammoOwner.IsOppositeEntity(mOwner.entityType))
                {
                    // Debug.Log($"{mOwner.name} shield destroy bullet of :[{hitBullet.ammoOwner}");
                    Destroy(other.gameObject);
                }else if (other.TryGetComponent<IDamageRecipient>(out var damageReceiver) && other.GetComponent<Spaceship>().entityType.IsOppositeEntity(mOwner.entityType))
                {
                    if (coolDownTable.ContainsKey(damageReceiver) && coolDownTable[damageReceiver]>0f)//还在cd
                    {
                        return;
                    }

                    // if (!coolDownTable.ContainsKey(damageReceiver))
                    // {
                    //     coolDownTable[damageReceiver] = damageCoolDown;//add
                    // }
                    coolDownTable[damageReceiver] = damageCoolDown;//add

                    // Debug.Log($"{mOwner.name} shield damage[{owner.shieldDamage}] {other.name}");
                    var afterStat = damageReceiver.BeingDamaged((int)owner.shieldDamage.ResultVal);
                    owner.DamagedCallback(afterStat);
                }
            }
        }
    }
}