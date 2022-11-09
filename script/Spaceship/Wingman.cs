using System;
using System.Collections.Generic;
using script.consts;
using script.DamageCalculate;
using script.Scene;
using script.Scriptable;
using script.Spaceship.Ability.Equip;
using script.Spaceship.Part;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace script.Spaceship
{
    [DefaultExecutionOrder(100)]
    public class Wingman : Spaceship
    {

        public Player linkedPlayer;

        public SpaceshipData attributeData;

        private float rotateSpeed = 50f;
        
        float workMinDuration = 3f;
        float workMaxDuration=8f;

        
        private float workDuration = 5f;
        private float workLeftTime = 0f;
        private bool isWorking;
        private int workIndex;
        private Action nowWork;
        public GameObject attackTarget;
        public Vector3 randomPoint;

        private const float screenLimitFactor = 0.15f;
        
        public void Init()
        {
            linkedPlayer = GlobalPlayerManger.Instance.Player;
            transform.position = GenerateRandomDestination();
            
            entityType = EntityType.Wingman;
            tag = Tags.Wingman;

            linkedPlayer = GlobalPlayerManger.Instance.Player;
            speed = attributeData.moveSpeed;
            bulletSpeed = attributeData.bulletSpeed;

            rotateSpeed = attributeData.rotateSpeed;
            maxHp = attributeData.maxHp;
            nowHp = attributeData.nowHp;
            baseDamage = attributeData.baseDamage;
            
            shieldDamage = attributeData.shieldDamage;
            timeSinceLastFire = attributeData.firstFireTime;
            fireCoolDown = attributeData.fireCoolDown;

            //weapon
            WeaponsController.EquipNewWeapon(ProjectVariables.Instance.normalWeapon.GetComponent<Weapon>());
            
            
            UpdateEvent += WingmanMoveAndAttack;
            UpdateEvent += TestDebug;
        }

        public bool IsInScreenView
        {
            get
            {
                var viewPoint = Camera.main.WorldToViewportPoint(transform.position);
                if (viewPoint.x is > screenLimitFactor and < 1-screenLimitFactor && viewPoint.y is > screenLimitFactor and < 1-screenLimitFactor)
                {
                    return true;
                }

                return false;
            }
        }
        public void TestDebug()
        {
            // Debug.Log(Camera.main.WorldToScreenPoint(transform.position));
        }
        #region Works Method
        private void StandAttack()
        {
            //朝向敌人
            if (attackTarget == null)
            {
                attackTarget=GameObject.FindGameObjectWithTag(Tags.Enemy);
            }

            if (attackTarget == null)
            {
                return;
            }
            //rotate to
            var direction = (attackTarget.transform.position - transform.position).normalized;
            if (Vector3.Distance(direction, transform.forward) > ProjectVariables.Instance.threshold)
            {
                var axis = Vector3.Cross(direction, transform.forward).normalized;
                // Debug.Log($"{direction} {transform.forward} {axis}");
                transform.Rotate(axis,-rotateSpeed*Time.deltaTime);
            }
            else
            {
                //开火
                timeSinceLastFire -= Time.deltaTime;
                if (timeSinceLastFire < 0f)
                {
                    WeaponsController.FireOnce();
                    timeSinceLastFire = fireCoolDown;
                }
            }
        }

        private int AroundTurnFactor = 1;
        // private float mAroundDistanceThreshold = 0f;

        private float AroundDistanceThreshold
        {
            get
            {
                if (linkedPlayer == null)
                {
                    return 0.5f;
                }
                return   linkedPlayer.GetComponent<MeshFilter>().mesh.bounds.size.magnitude*2;
            }  
        } 
        private void AroundPlayer()
        {
            if (Vector3.Distance(transform.position, linkedPlayer.transform.position) > AroundDistanceThreshold)
            {
                nowWork = MoveToPoint;
                return;
            }
            if (!IsInScreenView)
            {
                AroundTurnFactor *= -1;
            }

            // transform.forward = (transform.position - linkedPlayer.transform.position).normalized;
            transform.RotateAround(linkedPlayer.transform.position,ProjectVariables.Instance.fightPlaneNormal,AroundTurnFactor*rotateSpeed*Time.deltaTime);
        }

        private void MoveToPoint()
        {
            if (Vector3.Distance(transform.position, linkedPlayer.transform.position) < ProjectVariables.Instance.threshold)
            {
                nowWork = StandAttack;
                return;
            }
            transform.position = Vector3.Lerp(transform.position, randomPoint, Time.deltaTime * speed.GetResultValF());
        }

        #endregion

        private List<Action> works;
        /// <summary>
        /// move or attack
        ///3 type move types
        /// 1. stand attack
        /// 2. round player
        /// 3. move a direction
        /// </summary>
        private void WingmanMoveAndAttack()
        {
            if (works == null)
            {
                works = new();
                works.Add(StandAttack);
                works.Add(AroundPlayer);
                works.Add(MoveToPoint);
            }
            
            workLeftTime -= Time.deltaTime;
            if (workLeftTime > 0)
            {
               nowWork();
                return;
            }
            
            
            workDuration = Random.Range(workMinDuration, workMaxDuration);
            workLeftTime = workDuration;
            workIndex = Random.Range(0, works.Count);
            nowWork = works[workIndex];

            randomPoint = GenerateRandomDestination();
            isWorking = true;
        }
        
        private Vector3 GenerateRandomDestination()
        {
            Vector3 dest;
            while (true)
            {
                var fightPlaneHorizontal = Vector3.Cross(ProjectVariables.Instance.fightPlaneForward,
                    ProjectVariables.Instance.fightPlaneNormal);
                var direction=Random.Range(-1f, 1f) * fightPlaneHorizontal +
                              Random.Range(0, 1f) * ProjectVariables.Instance.fightPlaneForward;
                dest= linkedPlayer.transform.position + direction.normalized * 1f;
                dest.y = 0;

                if (Camera.main == null)
                {
                    return ProjectVariables.Instance.fightPlaneCenter + Vector3.down;
                }
                var viewPoint = Camera.main.WorldToViewportPoint(dest);
                if (viewPoint.x is > screenLimitFactor and < 1-screenLimitFactor && viewPoint.y is > screenLimitFactor and < 1-screenLimitFactor)
                {
                    break;
                }
            }
            
           
            return dest;
        }
    }
}