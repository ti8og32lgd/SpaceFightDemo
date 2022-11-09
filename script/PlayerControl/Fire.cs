using System;
using System.Collections;
using script.consts;
using script.input;
using script.Spaceship.Part;
using script.SpaceshipPart;
using UnityEngine;
using UnityEngine.InputSystem;

namespace script.PlayerControl
{
    public class Fire:MonoBehaviour
    {
         private bool _isFire;

         
         [Header("reference")]
         // private InputAction _firePressAction;
         public WeaponsController weaponsController;
        

         [SerializeField]private float coolDownTime=1f;
         private float _timeSinceLastFire;

       

         public void SetCoolDownTime(float cd)
        {
            coolDownTime = cd;
        }
        public void ListenFireAction(InputAction firePressAction)
        {
            firePressAction.started += _ =>
            {
                _isFire = true;
            };
            firePressAction.canceled += _ =>
            {
                _isFire = false;
            };
        }


        private void Update()
        {
            if (!_isFire)
            {
                return;
            }
            _timeSinceLastFire -= Time.deltaTime;//CD calculate
            
            if (_timeSinceLastFire < 0)
            {
                _timeSinceLastFire = coolDownTime;
                
                weaponsController.FireOnce();
            }
        }

        public void DebugFire()
        {
            _isFire = true;
        }
        public void CancelDebugFire()
        {
            _isFire = false;
        }
    }
}