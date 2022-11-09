using System.Collections.Generic;
using script.consts;
using script.SpaceshipPart;
using UnityEngine;

namespace script.Spaceship.Part
{
    public class PlayerWeaponsController:WeaponsController
    {
        
        public List<Vector3> weaponSlotsPos;//武器插槽位置（局部坐标）
        public int weaponMaxCount = 3;//or to say weapon slots

        public Weapon basicWeaponPrefab=>ProjectVariables.Instance.normalWeapon.GetComponent<Weapon>();

     
        
        protected override Weapon DoEquipNewWeapon(Weapon weaponPrefab)
        {
            if (weapons.Count >= weaponMaxCount)
            {
                return null;
            }

            var wpComponent= base.DoEquipNewWeapon(weaponPrefab);
            wpComponent.transform.localPosition =  GetNextWeaponSlot();
            // Debug.Log($"{  wpComponent.transform.localPosition }");
            return wpComponent;
        }




        public void AddWeaponSlotCount(int count)
        {
            SetWeaponSlotsCount(weaponSlotsPos.Count + count);
        }
        public void SetWeaponSlotsCount(int count)
        {
            weaponMaxCount = count;
            
            var colliderSize = GameObjectTransform.GetComponent<Collider>().bounds.size;
            
            
            weaponSlotsPos.Clear();
            
            float xStart = -colliderSize.x / 2,xStep=colliderSize.x/(1+count);                //slot沿着x轴平均分布
            // float yBottom = -colliderSize.y / 2;// 竖直方向放在机身腹部
            for (int i = 1; i <= count; i++)
            {
                weaponSlotsPos.Add(new Vector3(xStart+xStep*i,0,0)*(1f/GameObjectTransform.localScale.x));
            }
            
            
            //reassign weapons
            for (int i = 0; i < weapons.Count && i<weaponSlotsPos.Count; i++)
            {
                weapons[i].transform.position = weaponSlotsPos[i];
            }

            if (weaponSlotsPos.Count < weapons.Count)
            {
                for (int i = weaponSlotsPos.Count; i < weapons.Count; i++)
                {
                    MonoBehaviour.Destroy(weapons[i].gameObject);//destroy overflow weapons
                }
            
                weapons.RemoveRange(weaponSlotsPos.Count, weapons.Count-weaponSlotsPos.Count);
            }
            
        }
        //获取下一个武器插槽在飞船本体局部坐标系下的坐标 TODO
        private Vector3 GetNextWeaponSlot()
        {
            //TODO learn exception throw
            return weaponSlotsPos[weapons.Count];
        }

        public PlayerWeaponsController(Transform gameObjectTransform) : base(gameObjectTransform)
        {
            weaponSlotsPos = new List<Vector3>();
            SetWeaponSlotsCount(weaponMaxCount);
            // EquipNewWeapon(basicWeaponPrefab);
        }
    }
}