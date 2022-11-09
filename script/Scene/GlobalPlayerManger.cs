using System;
using script.consts;
using script.Scriptable;
using script.Spaceship.Ability.Equip;
using UnityEngine;

namespace script.Scene
{
    public class GlobalPlayerManger : Singleton<GlobalPlayerManger>
    {
        private Player mPlayer;
        public PlayerData playerInitData;
        private PlayerData playerRuntimeData;
        public Player Player
        {
            get
            {
                if (mPlayer == null) mPlayer = FindObjectOfType<Player>();
                return mPlayer;
            }
        }


        /// <summary>
        /// 将当前场景内player替换为全局Player
        /// </summary>
        public void ReplacePlayerInSceneWithGlobal()
        {
            
            var findPlayers=FindObjectsOfType<Player>();
            if (findPlayers.Length > 2)
            {
                Debug.LogError("Player Count > 2! GlobalPlayerManger can't handle it");
                throw new InvalidProgramException();
            }

            if (findPlayers.Length == 1) return;
            
            Player global = null, local=null;
            foreach (var p in findPlayers)
            {
                if (p == mPlayer)
                {
                    // p.name = "global";
                    global = p;
                }
                else
                {
                    // p.name = "local";
                    local = p;
                }
            }
            
            //
            global.transform.SetPositionAndRotation(local.transform.position,local.transform.rotation);
            global.transform.SetParent(local.transform.parent);
            Destroy(local.gameObject);
        }
        
        public void InitPlayer()
        {
            var player = Player;
            
            //set value
            player.entityType = EntityType.Player;
            player.tag = Tags.Player;
            player.speed = playerInitData.moveSpeed;
            player.bulletSpeed = playerInitData.bulletSpeed;
            
            player.baseDamage = playerInitData.baseDamage;
            player.shieldDamage = playerInitData.shieldDamage;
            player.timeSinceLastFire = playerInitData.firstFireTime;
            player.fireCoolDown = playerInitData.fireCoolDown;
            player.maxHp = playerInitData.maxHp;
            player.nowHp = playerInitData.nowHp;

            player.WeaponsController.ClearWeapons();
            player.WeaponsController.EquipNewWeapon(playerInitData.initWeapon);
             //equip normal weapon(obsolect=> set in player script)
            // var success=player.WeaponsController.EquipNewWeapon(playerInitData.initWeapon);
            // if (!success)
            // {
            //     Debug.LogError("player equip init weapon failed.");
            // }
            
            playerRuntimeData = Instantiate(playerInitData);

            // InitDebug();
        }

        private void InitDebug()
        {
            Player.EquipSpaceshipAbility(new AutoAimEquip());
        }

        private void Start()
        {
            // InitPlayer();
        }
    }
}