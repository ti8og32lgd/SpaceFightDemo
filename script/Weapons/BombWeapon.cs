using script.consts;
using script.Spaceship.Part;
using script.SpaceshipPart;
using UnityEngine;

namespace script.Weapons
{
    /// <summary>
    /// 爆炸的武器，相比普通武器，发射的更少
    /// 每round次开火发射一次
    /// </summary>
    public class BombWeapon : Weapon
    {

        public int round = 3;
        public int nowRound = 0;
        public override void Fire()
        {
            if (nowRound < round)
            {
                nowRound++;
                return;
            }

            nowRound %= round;
            var bulletInstance=Instantiate(bulletPrefab, _transform.position, _transform.rotation);
            
            // bulletInstance.transform.SetParent(_transform);
            bulletInstance.GetComponent<Bullet>().SetBullet(owner.baseDamage,owner.bulletSpeed,owner.entityType,owner.InvokeOnCauseDamage);
            ManualAfterInstantiateBullet(this, bulletInstance.GetComponent<Bullet>());
            
            //TODO 绑定子弹的伤害callback，初始化子弹伤害
                    
            //播放声音 //TODO put in unique method 
            fireAudio.PlayOneShot(fireAudioClip);
        }
    }
}