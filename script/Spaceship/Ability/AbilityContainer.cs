using System;
using System.Collections.Generic;
using script.Spaceship.Ability.Equip;
using UnityEngine;
using Random = UnityEngine.Random;

namespace script.Spaceship.Ability
{
    
   
    
    
    public  class AbilityContainer : Singleton<AbilityContainer>
    {

        private static SpaceshipAbilityEquip[] equips =
        {
            new InvincibleShieldEquip(),
            new RecycleHealEquip(),
            new SlowGlobalTimeEquip(),
            new WingmanEquip(),
            new AutoAimEquip(),
        };
        public bool[] isUsed=new bool[equips.Length];

        /// <summary>
        /// 获取count个还未被player使用的AbilityEquip的info
        /// </summary>
        /// <returns></returns>
        public List< Tuple<int,AbilityIntro>> GetUnusedAbilityEquipInfo(int count)
        {
            List< Tuple<int,AbilityIntro>>  ret = new();

            for (var i = 0; i <equips.Length; i++)
            {
                if (count <= 0)
                {
                    break;
                }
                if (!isUsed[i])
                {
                   
                    ret.Add(new Tuple<int, AbilityIntro>(i,equips[i].GetIntro()));
                    count--;
                }
            }
            
            return ret;
        }
        
        public SpaceshipAbilityEquip  UseAbilityEquip(int index)
        {
            isUsed[index] = true;
            return equips[index];
        }
        
    }
}