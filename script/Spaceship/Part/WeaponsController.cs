using System.Collections.Generic; 
using UnityEngine;

namespace script.Spaceship.Part
{
    public class WeaponsController
    {
        public readonly Transform GameObjectTransform;
        
          
        public List<Weapon> weapons=new();
        // public List<Vector3> weaponSlotsPos;//武器插槽位置（局部坐标）
        // public int weaponMaxCount = 1;//or to say weapon slots

       
        public delegate void HandleWeapon(Weapon weapon);
        public event HandleWeapon weaponEquipEvent;//增加武器时调用处理武器

        public WeaponsController(Transform gameObjectTransform)
        {
            this.GameObjectTransform = gameObjectTransform;
        }

        
        public void FireOnce()
        {
            foreach (var w in weapons)
            {
                w.Fire();
            }
           
        }

        public void ClearWeapons()
        {
            foreach (var weapon in weapons)
            {
                GameObject.Destroy(weapon.gameObject);
            }
        }
        public  bool EquipNewWeapon(Weapon weaponPrefab)
        {
           Weapon wpComp= DoEquipNewWeapon(weaponPrefab);
           if (wpComp == null) return false;

           wpComp.owner = GameObjectTransform.GetComponent<Spaceship>();
           
           
           weaponEquipEvent?.Invoke(wpComp);
           
           weapons.Add(wpComp);
           
           
           return true;
        }
       
        protected virtual Weapon DoEquipNewWeapon(Weapon weaponPrefab)
        {
            // Debug.Log(GameObjectTransform);
            GameObject  newWeaponObj=GameObject.Instantiate(weaponPrefab.gameObject,GameObjectTransform);
            var newWeapon = newWeaponObj.GetComponent<Weapon>();
            newWeapon.transform.localPosition=Vector3.zero;
            newWeapon.transform.localRotation=Quaternion.identity;
            return newWeapon;
        }




      
    }
}