
using System;
using System.Collections;
using script.Spaceship.Part;
using UnityEngine;

namespace script.Weapons
{
    public class LaserWeapon : Weapon
    {
        //sub game object
        [SerializeField]
        private GameObject laserObj;  //射线（直线）obj

        [SerializeField]private bool isFiring;//是否正在开火
        float coolDownTime=1f;
        private float timeSinceLastFire;
        private void Start()
        {
            _transform = transform;//basic.start
            

            laserObj.GetComponent<LaserBullet>().ammoOwner = owner.entityType;

        }
        
        public override void Fire()
        {
            timeSinceLastFire = coolDownTime;
            isFiring = true;
            laserObj.SetActive(true);
            
        }

        private void Update()
        {
            timeSinceLastFire -= Time.deltaTime;
            if (timeSinceLastFire<0f)
            {
                laserObj.SetActive(false);
                isFiring = false;
            }
        }

       
      
        
        
    }
}