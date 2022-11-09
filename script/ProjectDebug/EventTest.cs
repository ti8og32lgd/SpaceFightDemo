using System;
using System.Collections;
using System.Collections.Generic;
using script.consts;
using script.PlayerControl;
using script.ProjectDebug;
using script.Spaceship;
using script.Spaceship.Ability;
using script.Spaceship.Ability.Equip;
using script.Spaceship.Part;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace script.SpaceshipPart
{
    public class EventTest : MonoBehaviour
    {
        [Header("binding gameobj")]
        public Player Player;
        public Weapon ExampleWeapon;
        public Enemy Enemy;
        [Header("binding ui")]
        public TMP_Dropdown bulletType;
        public TMP_Dropdown abilityOptions;
        // public Slider playerFireSpeedSlider; 
            
        [Header("Script object")] 
        public DebugObjects abilityPrefabLinks;
        public DebugObjects bulletPrefabLinks;
        [Header("prefabs")]
        public GameObject bulletPrefab;

        public Weapon normalWeaponPrefab;
        public Weapon laserWeaponPrefab;
        private Vector3 originalPos;
        private bool ControlPlayerMoveFlag;

        private void Start()
        {
            originalPos = Player.transform.position;
            InitAbilityDropdown();
            InitBulletDropDown();
            AssignBulletType(0);
        }

        public void InitAbilityDropdown()
        {
            abilityOptions.options = new List<TMP_Dropdown.OptionData>();
           
            for(var i=0;i<abilityPrefabLinks.prefabLinks.Length;i++)
            {
                abilityOptions.options.Add( new TMP_Dropdown.OptionData(abilityPrefabLinks.prefabLinks[i].name));
            }
        }

        public void InitBulletDropDown()
        {
            bulletType.options = new List<TMP_Dropdown.OptionData>();
           
            for(var i=0;i<bulletPrefabLinks.prefabLinks.Length;i++)
            {
                bulletType.options.Add( new TMP_Dropdown.OptionData(bulletPrefabLinks.prefabLinks[i].name));
            }
        }

        public void  SetPlayerFireSpeed(float val)
        {
            var cd = val;
            Player.GetComponent<Fire>().SetCoolDownTime(1/cd);
        }
        public void PlayerAutoFire()
        {
            Player.GetComponent<Fire>().DebugFire();
        }
        
        public void AddOneNormalWeapon()
        {
            normalWeaponPrefab.GetComponent<Weapon>().bulletPrefab = bulletPrefab;
            Player.WeaponsController.EquipNewWeapon(normalWeaponPrefab);
        }
        
        public void AddOneLaserWeapon()
        {
            
            Player.WeaponsController.EquipNewWeapon(laserWeaponPrefab);
        }

        public void AddOneWeaponSlot()
        {
            PlayerWeaponsController wp=(PlayerWeaponsController)Player.WeaponsController;
              wp.AddWeaponSlotCount(1);
        }
        public void MinusOneWeaponSlot()
        {
            PlayerWeaponsController wp=(PlayerWeaponsController)Player.WeaponsController;
            wp.AddWeaponSlotCount(-1);
        }

        public void ShootOneBullet()
        {
            // var choose = bulletType.value;
            ExampleWeapon.bulletPrefab = bulletPrefab;//set prefab bullet
            ExampleWeapon.Fire();
        }

        public void SelectMoveMethod(int i)
        {
            // Debug.Log("SelectMoveMethod "+i);
            switch (i)
            {
                case 0://静止
                    
                    if (ControlPlayerMoveFlag != false)
                    {
                        // Debug.Log("StopCoroutine  "+i);
                        ControlPlayerMoveFlag = false;
                        StopCoroutine("ControlPlayerMove");
                    }
                    break;
                case 1: //左右cos
                case 2://绕原点
                    if (ControlPlayerMoveFlag != false)
                    {
                        // Debug.Log("StopCoroutine  "+i);
                        ControlPlayerMoveFlag = false;
                        StopCoroutine("ControlPlayerMove");
                    }
                    ControlPlayerMoveFlag = true;
                   
                    StartCoroutine(ControlPlayerMove(i));
                    break;
            }
        }

        IEnumerator ControlPlayerMove(int moveType)
        {
            while (ControlPlayerMoveFlag)
            {
                float x=(0), z=0;
                if (moveType == 1)
                {
                    x = Mathf.Cos(Time.time);
                }else if (moveType == 2)
                {
                    x = Mathf.Cos(Time.time);
                    z = Mathf.Sin(Time.time);
                }
                Player.transform.position = originalPos+new Vector3(x, 0, z);
                yield return new WaitForFixedUpdate();
            }
            // Debug.Log($"Go out of moveType{moveType}");
            yield return null;
        }

        public void GiveNewAbility(string type)
        {
            var prefab = abilityPrefabLinks.prefabLinks[abilityOptions.value].prefab;
            prefab.GetComponent<SpaceshipAbilityEquip>().Do(type=="player"?Player:Enemy);
        }
        
        public void AssignBulletType(int  index)
        {
            var prefab = bulletPrefabLinks.prefabLinks[index].prefab;
            bulletPrefab = prefab;
        }
        
        public bool SetEnemy2
        {
            set
            {
                Enemy.gameObject.SetActive(value);
            }
        }
     
    }
}