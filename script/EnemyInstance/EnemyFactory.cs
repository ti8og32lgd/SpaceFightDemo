using System;
using script.consts;
using script.Scriptable;
using script.Spaceship;
using script.Spaceship.Part;
using script.UI;
using script.Weapons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.EnemyInstance
{
    public static class EnemyFactory 
    {
        

        public static GameObject CreateEnemy(EnemyData enemyData)
        {
            var spaceshipObj=Resources.Load(enemyData.spaceshipModulePath) as GameObject;//TODO scale 问题
            if (spaceshipObj == null) return null;
            // var weaponObj = Resources.Load(enemyData.weaponModulePath) as GameObject;
            // if (weaponObj == null) weaponObj = new GameObject("no module weapon");
            // var bulletObj=Resources.Load(enemyData.bulletModulePath) as GameObject;
            // if (bulletObj == null) bulletObj = new GameObject("no module bullet");//TODO 默认子弹类型

            spaceshipObj = MonoBehaviour.Instantiate(spaceshipObj);
            //Collider 
            var meshCollider= spaceshipObj.AddComponent<MeshCollider>();
            // spaceshipObj.GetComponent<MeshFilter>().mesh.isReadable = true;
            meshCollider.sharedMesh = spaceshipObj.GetComponent<MeshFilter>().mesh;
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
            
            
            var enemy  =spaceshipObj.AddComponent<Enemy>();
            
            //Hpbar
            spaceshipObj.AddComponent<HpBarHost>();
            
            //property
            enemy.entityType = EntityType.Enemy;
            enemy.tag = Tags.Enemy;
            enemy.name = "enemy "+enemyData.name+" " + Time.time;
            enemy.rotSpeed = enemyData.rotateSpeed;
            enemy.speed = enemyData.moveSpeed;
            enemy.bulletSpeed = enemyData.bulletSpeed;
            
            enemy.baseDamage = enemyData.baseDamge;
            enemy.shieldDamage = enemyData.shellDamge;
            enemy.timeSinceLastFire = enemyData.firstFireTime;
            enemy.fireCoolDown = enemyData.fireCoolDown;
            enemy.maxHp = enemyData.maxHp;
            enemy.nowHp = enemyData.nowHp;
            //move 
            var moveMethod = enemyData.moveType.GetMethod(); 
            enemy.UpdateEvent +=()=> moveMethod(enemy);
            //attack
            var attackMethod = enemyData.attackType.GetMethod();
            enemy.UpdateEvent += () => attackMethod(enemy);

            var weaponPrefabToAdd = enemyData.weaponType.GetWeapon();
            weaponPrefabToAdd.SetVolume(enemyData.volume);
            
            
            var fireAudioClip=Resources.Load(enemyData.shootAudioClipPath) as AudioClip;//TODO scale 问题
            if (spaceshipObj == null) fireAudioClip=ProjectVariables.Instance.defaultFireAudioClip;
            weaponPrefabToAdd.fireAudioClip = fireAudioClip;
                
            
            enemy.AfterStartEvent += ()=>{
                // enemy.WeaponsController.
                enemy.WeaponsController.EquipNewWeapon(weaponPrefabToAdd);
            };
            if (enemyData.isFindPlayerAsTarget)
            {
                enemy.AfterStartEvent += () =>
                {
                    enemy.attackTarget= GameObject.Find("Player");
                };
            }
            

            return enemy.gameObject ;
        }

        public enum WeaponType
        {
            NormalWeapon,BombWeapon,LaserWeapon
        }
        public enum MoveType
        {
            StraightForward,GoMidAndLookAtTarget,TowardTarget
           
        }
        public enum AttackType
        {
            IntervalFire
        }

        public static Weapon GetWeapon(this WeaponType type)
        {
            return ProjectVariables.Instance.normalWeapon.GetComponent<Weapon>();
        }
        public static Action<Enemy> GetMethod(this MoveType type)
        {
            switch (type)
            {
                case MoveType.StraightForward:
                    return MoveMethods.StraightForward;
                case MoveType.GoMidAndLookAtTarget:
                    return MoveMethods.GoMidAndLookAtTarget;
                case MoveType.TowardTarget:
                    return MoveMethods.TowardTarget;
            }
             return MoveMethods.StraightForward;
        }
        public static Action<Enemy> GetMethod(this AttackType type)
        {
            switch (type)
            {
                case AttackType.IntervalFire:
                    return AttackMethods.IntervalFire;
                
            }
            return AttackMethods.IntervalFire;
        }
        /// <summary>
        ///Enemy移动方式
        /// </summary>
        public static class MoveMethods
        {
            /// <summary>
            /// 直线向前
            /// </summary>
            public static readonly Action<Enemy> StraightForward= em =>
            {
                em.transform.Translate(Vector3.forward * ((float)em.speed.ResultVal * Time.deltaTime));
            };

            /// <summary>
            /// 移动到地图竖向中间(z=?)，然后朝向目标
            /// </summary>
          public static  Action<Enemy> GoMidAndLookAtTarget
            {
                get
                {
                    const float threshold = 0.1f;
                    //generate a horizontal line value
                    var camera = Camera.main;
                    var  mainScaledPixelHeight = camera!.scaledPixelHeight *Random.Range(0.25f, 0.75f);//屏幕的25%到75%
                    var randZ=camera.ScreenToWorldPoint(new Vector3(0,mainScaledPixelHeight, 0)).z;

                    int stage = 1;
                    return em =>
                    {
                        var transform = em.transform;
                        // var position = transform.position;
                        var movDelta = (float)em.speed.ResultVal * Time.deltaTime;
                        var rotDelta=em.rotSpeed* Time.deltaTime;
                        var originPos = transform.position;//original pos this frame
                        if(stage==1) //第一阶段，驶向中央 （z==randZ)
                        {
                            var targetPos = originPos;
                            targetPos.z = randZ;
                            var nextPos=Vector3.MoveTowards(originPos,targetPos,movDelta);
                            transform.position = nextPos;
                            if (Mathf.Abs(originPos.z - randZ) < threshold)
                            {
                                stage = 2;
                            }
                        }
                        else if (stage==2)//第二阶段，look at target
                        {
                            if (em.attackTarget == null)
                            {
                                em.transform.Translate(Vector3.forward * movDelta);
                            }
                            else
                            {
                                var dir =(em.attackTarget.transform.position - originPos).normalized;
                                // dir = Quaternion.AngleAxis(120, transform.forward);
                                transform.rotation= Quaternion.RotateTowards( transform.rotation, Quaternion.LookRotation(dir),rotDelta);
                            }
                        }
                    };
                }
            }


            public static readonly Action<Enemy> TowardTarget = em =>
            {
                if (em.attackTarget == null)
                {
                    em.transform.Translate(Vector3.forward * ((float)em.speed.ResultVal * Time.deltaTime));
                    return;
                }

                Transform transform =   em.transform;
                Transform targetTransform = em.attackTarget.transform;
               

                var dir = (targetTransform.position - transform.position).normalized;
                var rotDelta = em.rotSpeed * Time.deltaTime;
                transform.rotation= Quaternion.RotateTowards( transform.rotation, Quaternion.LookRotation(dir),rotDelta);

                if ((targetTransform.position - transform.position).magnitude < 2.5f)
                {
                    return;
                }
                
                var forward=transform.InverseTransformDirection(transform.forward);
                transform.Translate( forward * ((float)em.speed.ResultVal * Time.deltaTime));
            };
        }

        public static class AttackMethods
        {
            public static readonly Action<Enemy> IntervalFire = (em) =>
            {
                em.timeSinceLastFire -= Time.deltaTime;

                if (em.timeSinceLastFire <= 0)
                {
                    em.WeaponsController.FireOnce();
                    em.timeSinceLastFire = em.fireCoolDown;
                }
            };
        }
   

      
    }

  
}