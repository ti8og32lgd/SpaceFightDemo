using script.Spaceship;
using script.Spaceship.Part;
using UnityEngine;

namespace script.EnemyInstance
{
    /// <summary>
    /// 普通敌人类型1
    ///     移动：直线向前走
    ///     开火：直线发射
    ///     子弹：普通子弹
    /// </summary>
    ///
    ///
    [DefaultExecutionOrder(-1)]
    public class EnemyTypeNormal1 : MonoBehaviour
    {
        private Enemy em;
        public Weapon normalWeaponPrefab;
        private float cd = 1.0f,fireTime=0f;
        private void Start()
        {
            em = GetComponent<Enemy>();
            // em.UpdateEvent +=()=> EnemyFactory.MoveMethods.StraightForward(em);
            var moveAction = EnemyFactory.MoveMethods.GoMidAndLookAtTarget;
            em.UpdateEvent +=()=>moveAction(em);
            em.UpdateEvent += Fire;
            em.AfterStartEvent += EquipNormalWeapon;            
            em.AfterStartEvent += FindPlayerAsAttackTarget;
            // Debug.Log($"Start {name}");
        }

        private void FindPlayerAsAttackTarget()
        {
           em.attackTarget= GameObject.Find("Player");
           // Debug.Log($" em.attackTarget= {em.attackTarget};");
        }

        private void Fire()
        {
            fireTime -= Time.deltaTime;
            if (fireTime <= 0)
            {
                em.WeaponsController.FireOnce();
                fireTime = cd;
            }

        }
        

        private void EquipNormalWeapon()
        {
            if (!em.WeaponsController.EquipNewWeapon(normalWeaponPrefab))
            {
                Debug.Log($"{name} equip weapon {normalWeaponPrefab.name} fail");
            }
        }
    }
}