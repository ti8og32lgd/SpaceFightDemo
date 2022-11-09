using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using script.Buffer;
using script.consts;
using script.Scene;
using script.Scriptable;
using script.Spaceship.Part;
using script.UI;
using UnityEngine;

namespace script.Spaceship.Prop
{
    public static class PropGenerator
    {
        public  static GameObject CreatePropGameObject(PropData propData)
        {
            GameObject obj = MonoBehaviour.Instantiate(propData.ObjPrefab);
            obj.tag = Tags.Prop;
            obj.name = $"[prop {propData.type}] {propData.value} ";
            Debug.Log($"Create Prop {obj.name}");
            
            var propComp=obj.AddComponent<Prop>();
            var action = propData.type.GetPropAction(propData.value);
            propComp.isRotate = propData.isObjRotate;
            propComp.OnPick += action;
            propComp.OnPick += (_) =>
            {
                var obj = GameObject.FindObjectOfType<FightScene>();
                if (obj != null)
                {
                    obj.PlayDialog($"拾取道具:{propData.type}");
                }
            };
            return obj;
        }

        
        public enum PropType
        {
            HpAdd,HpMulti,DamageAdd,DamageMulti,SpeedAdd,SpeedMul,AddNormalWeapon,AddBombWeapon,AddLaserWeapon
        }

        private static Dictionary<PropType, Func<float, Action<Spaceship>>> propsTypeTable = new()
        {
            {PropType.HpAdd,HpAdd},
            {PropType.HpMulti,HpMulti},
            
            {PropType.DamageAdd,DamageAdd},    
            {PropType.DamageMulti,DamageMul},   
                                
            {PropType.SpeedAdd,SpeedAdd},     
            {PropType.SpeedMul,SpeedMul},     
                                
            {PropType.AddNormalWeapon,AddNormalWeapon},
            {PropType.AddBombWeapon,AddBombWeapon},
            {PropType.AddLaserWeapon,AddLaserWeapon},
        };


        
       
    
        private static Action<Spaceship> GetPropAction(this PropType type,float value)
        {
            return propsTypeTable[type](value);
        }

        private static  Action<Spaceship> HpAdd(float value)
        {
            return spaceship =>
            {
                spaceship.maxHp +=(int) value;
                // Debug.Log($"{spaceship.GetComponent<PlayerHpBar>()}");
                if (spaceship.TryGetComponent<PlayerHpBar>(out var playerHpBar))
                {
                    // Debug.Log($"{playerHpBar.bar}");

                    playerHpBar.bar.SetFullHpAndRefreshBar(spaceship.maxHp);
                    Debug.Log($"SetFullHpAndRefreshBar done");

                }
                spaceship.nowHp +=(int) value;
                spaceship.InvokeOnLifeChanged();
                Debug.Log($"InvokeOnLifeChanged done");

            };
        }
        
        private static  Action<Spaceship> HpMulti(float value)
        {
            return spaceship =>
            {
                
                spaceship.maxHp = (int)(spaceship.maxHp*value);
                if (spaceship.TryGetComponent<PlayerHpBar>(out var playerHpBar))
                {
                    playerHpBar.bar.SetFullHpAndRefreshBar(spaceship.maxHp);
                }
                
                spaceship.nowHp = (int)(spaceship.nowHp*value);
                spaceship.InvokeOnLifeChanged();
            };
        }

        private static Action<Spaceship> SpeedAdd(float value)
        {
            return spaceship =>
            {
                spaceship.speed.AddBufferUnit(new BufferUnit("SpeedAddProp","SpeedAddProp",BufferUnit.BufferType.Add,(decimal)value));
            };
        }
        
        private static Action<Spaceship> SpeedMul(float value)
        {
            return spaceship =>
            {
                spaceship.speed.AddBufferUnit(new BufferUnit("SpeedMulProp","SpeedMulProp",BufferUnit.BufferType.Mul,(decimal)value));
            };
        }
        
        private static Action<Spaceship> DamageAdd(float value)
        {
            return spaceship =>
            {
                spaceship.baseDamage.AddBufferUnit(new BufferUnit("DamageAddProp","DamageAddProp",BufferUnit.BufferType.Mul,(decimal)value));
            };
        }
        
        private static Action<Spaceship> DamageMul(float value)
        {
            return spaceship =>
            {
                spaceship.baseDamage.AddBufferUnit(new BufferUnit("DamageMulProp","DamageMulProp",BufferUnit.BufferType.Mul,(decimal)value));
            };
        }
        
        private static Action<Spaceship> AddNormalWeapon(float value)
        {
            return spaceship =>
            {
                spaceship.WeaponsController.EquipNewWeapon(ProjectVariables.Instance.normalWeapon.GetComponent<Weapon>());
            };
        }
        
        private static Action<Spaceship> AddBombWeapon(float value)
        {
            return spaceship =>
            {
                spaceship.WeaponsController.EquipNewWeapon(ProjectVariables.Instance.bombWeapon.GetComponent<Weapon>());
            };
        }
        
        private static Action<Spaceship> AddLaserWeapon(float value)
        {
            return spaceship =>
            {

            };
        }

        
    }
}