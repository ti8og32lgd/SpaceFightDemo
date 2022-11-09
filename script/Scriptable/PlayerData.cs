using script.Spaceship.Part;
using UnityEngine;

namespace script.Scriptable
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "data/PlayerData", order = 0)]
    public class PlayerData : SpaceshipData
    {
        public Weapon initWeapon;

    }
}