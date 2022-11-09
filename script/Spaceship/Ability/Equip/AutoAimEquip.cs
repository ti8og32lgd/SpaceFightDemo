using script.Spaceship.Part;
using UnityEditor;
using UnityEngine;

namespace script.Spaceship.Ability.Equip
{
    public class AutoAimEquip:SpaceshipAbilityEquip
    {
        private readonly AbilityIntro intro = new ("自动瞄准", "使你的子弹能够自动跟踪敌人","Images/autoaim");
        public override AbilityIntro GetIntro()
        {
            return intro;
        }

      
        public override void Do(Spaceship player)
        {
            
            Debug.Log("Get AutoAim!");
            //AddComponent Aim
            //all bullet of weapons until now
            foreach (var weapon in player.WeaponsController.weapons)
            {
                TryAddComponentAutoAim(weapon);
            }
            
            //when equip new 
            player.WeaponsController.weaponEquipEvent += TryAddComponentAutoAim;
        }

        public override void Remove(Spaceship player)
        {
            foreach (var weapon in player.WeaponsController.weapons)
            {
                TryRemoveComponentAutoAim(weapon);
            }

            player.WeaponsController.weaponEquipEvent -= TryAddComponentAutoAim;
        }

        private static void TryAddComponentAutoAim(Weapon weapon)
        {
            weapon.AfterInstantiateBullet += (w, b) =>
            {
                if (b.GetComponent<Weapons.AutoAim>()==null)
                {
                    b.gameObject.AddComponent<Weapons.AutoAim>();
                }
            };
        }
        
        private static void TryRemoveComponentAutoAim(Weapon weapon)
        {
            //TODO
            // if (weapon.bulletPrefab.GetComponent<Weapons.AutoAim>()!=null)
            // {
            //     MonoBehaviour.Destroy(weapon.bulletPrefab.GetComponent<Weapons.AutoAim>());
            // }
        }
    }
}