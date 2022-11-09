using script.consts;
using script.SpaceshipPart;
using UnityEngine;

namespace script.Spaceship.Part
{
    public class EnemyWeaponController:WeaponsController
    {
        protected override Weapon DoEquipNewWeapon(Weapon weaponPrefab)
        {
            var wp= base.DoEquipNewWeapon(weaponPrefab);
            // wp.SetVolume( 0.1f);
            return wp;
        }

        public EnemyWeaponController(Transform gameObjectTransform) : base(gameObjectTransform)
        {
        }
    }
}