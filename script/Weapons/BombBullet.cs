using script.Buffer;
using script.consts;
using script.DamageCalculate;
using script.Spaceship;
using UnityEngine;

namespace script.Weapons
{
    public class BombBullet:Bullet
    {

        public float explodeRadius=1.5f;
        public GameObject explodeFXPrefab;
        private BufferValue lagSpeed;
        public float lagSpeedFactor = 0.3f;
        public BombBullet()
        {
            lagSpeed = speed;
            lagSpeed.AddBufferUnit(new BufferUnit("lag","爆炸弹，该跑的慢一些",BufferUnit.BufferType.Mul,(decimal)lagSpeedFactor));

            liveTime /= lagSpeedFactor;
            // Debug.Log("NormalBullet init with m_translateUpdate");
            MTranslateUpdate = () =>
            {
                MTransform.Translate ( Vector3.forward * (lagSpeed.GetResultValF() * Time.deltaTime));
                //左右轻微摇晃
                MTransform.position+=Vector3.left * (Time.deltaTime * Mathf.Cos(Time.time*8f))*0.1f;
                //自己旋转
                MTransform.Rotate(transform.forward,45*Time.deltaTime,Space.Self);
            };
        }

        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name+" "+other.tag);
            if (!other.TryGetComponent<Spaceship.Spaceship>(out var sp)) return;

            if (!other.TryGetComponent<IDamageRecipient>(out var damageRecipient)) return;
            
            if (sp.entityType.IsOppositeEntity(ammoOwner))
            {
                ExplodeCollideDetect();
                StartExplodeEffect();
                Destroy(gameObject);
            }
        }
        
        
        public  void ExplodeCollideDetect()
        {
            Debug.Log("explode scan");

            //SphereLap Collider
            Collider[] colliders = new Collider[] { };
            // var size = Physics.OverlapSphereNonAlloc(m_transform.position, explodeRadius,colliders, LayerMask.GetMask("Spaceship"));
            colliders = Physics.OverlapSphere(MTransform.position, explodeRadius);
            foreach (var other in colliders)
            {
                if (!other.CompareTag(ammoOwner.GetEntityTag()))//cause damage;
                {
                    if (other.TryGetComponent<IDamageRecipient>(out var damageReceiver))
                    {
                        HitOtherEntity(damageReceiver, GiveDamage());
                    }
                }
            }
        }
        public override void HitOtherEntity(IDamageRecipient damageReceiver,  int damage)
        {
            var afterStat = damageReceiver.BeingDamaged(damage);
            ManualInvokeAfterDamageCallback(afterStat);
        }

        public void StartExplodeEffect()
        {
            var explodeObj=Instantiate(explodeFXPrefab,MTransform.position,Quaternion.identity);
            explodeObj.GetComponent<BombEffect>().originScale*=explodeRadius;
        }
    }
}