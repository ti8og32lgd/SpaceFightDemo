using script.consts;
using script.Spaceship.Part;
using script.SpaceshipPart;
using UnityEngine;

namespace script.Weapons
{
    /// <summary>
    /// 一次发射一个子弹
    /// </summary>
    public class NormalWeapon : Weapon
    {
        
        public override void Fire()
        {
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