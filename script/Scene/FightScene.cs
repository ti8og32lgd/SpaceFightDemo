using System;
using System.Collections;
using System.Collections.Generic;
using script.Scriptable;
using script.Spaceship.Ability.Equip;
using script.Spaceship.Part;
using script.Spaceship.Prop;
using script.Spawn;
using script.UI;
using script.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace script.Scene
{
    public class FightScene : MonoBehaviour
    {
        
        
        public enum  GameStatus
        {
            Fighting,Paused
        }
        public GameStatus status;
        //战斗父物体
        public Transform fightPlane;
        [Header("UI bind")]
        public Canvas pauseCanvas;
        public HPBar HpBar;
        public TextMeshProUGUI nowHpTxt, fullHpTxt;
        public TextMeshProUGUI gameTimeTxt,gameLevelTxt;
        public RectTransform levelBeginBanner, levelPassBanner, levelFailBanner;
        public RectTransform abilityPanel, weaponPanel;
        public TextMeshProUGUI dialogTxt;
        public Sprite NormalWeaponSprite, BombWeaponSprite, LaserWeaponSprite;
        [Header("UI prefab")] public GameObject abilityIconPrefab, weaponIconPrefab;
        [Header("value")] public float BannerDisplayTime = 2.5f;
        [Header("player setting")]
        public Player player;
        public Vector3 playerInitPos;

        [Header("Spawn")] public PropSpawnManger propSpawnManger;

       
        private void Start()
        {
            GlobalPlayerManger.Instance.ReplacePlayerInSceneWithGlobal();
            pauseCanvas.gameObject.SetActive(false);
            
            PlayerInitSetting();
            //
           StartCoroutine(DisplayLevelBeginBanner());
            BeginSpawn();
            
            GenAbilityPanel();
            GenWeaponPanel();
            gameLevelTxt.text = $"第{GameManager.Instance.level}关";
            Debug.Log(gameLevelTxt.text );
            GlobalPlayerManger.Instance.Player.WeaponsController.weaponEquipEvent += AddWeaponIcon;
            //test prop
            // PropData testPropData=ProjectVariables.Instance.testPropData;
            // var PropObj=PropGenerator.CreatePropGameObject(testPropData);
            // PropObj.transform.position = Vector3.zero;
        }

        private float levelTime=0f;
        private void Update()
        {
            levelTime += Time.deltaTime;
            gameTimeTxt.text = $"时间：{levelTime.ToString("0.00")}";
        }


        public void PauseFightScene()
        {
            Debug.Log(status);
            switch (status)
            {
                case GameStatus.Fighting:
                    Time.timeScale = 0f;
                    pauseCanvas.gameObject.SetActive(true);
                    status = GameStatus.Paused;
                    break;
                case GameStatus.Paused:
                    Time.timeScale = 1f;
                    pauseCanvas.gameObject.SetActive(false);
                    status = GameStatus.Fighting;
                    break;
            }
           
        }

        private void PlayerInitSetting()
        {

            //0.找到player
            player = GlobalPlayerManger.Instance.Player;
            player.transform.position = playerInitPos;
            //1.归还player控制
            player.EnableInput();
            //2. player 血条初始化

            var bar=player.gameObject.AddComponent<PlayerHpBar>();
            bar.bar = HpBar;
            bar.fullHpTxt = fullHpTxt;
            bar.nowHpTxt = nowHpTxt;
            bar.Init();    
            Debug.Log($"PlayerInitSetting:PlayerHpBar={bar}  HpBar={HpBar}");

        }
        /// <summary>
        /// 开始生成敌人、道具
        /// </summary>
        /// <param name="level"></param>
        public void BeginSpawn()
        {

            var spawnSetting = ProjectVariables.Instance.LevelEnemySpawnSettings[GameManager.Instance.level];
            int i = 0;
            foreach (var setting in spawnSetting.settings)
            {
                var spawnObj=Instantiate(setting.spawnManager, fightPlane);
                spawnObj.name = $"level {GameManager.Instance.level} spawn {i++}";
                spawnObj.transform.position = setting.data.initSpawnPosition;
                Debug.Log(spawnObj.transform.position+" "+setting.data.initSpawnPosition);

                spawnObj.spawnData = setting.data;
                spawnObj.BeginSpawn();
                spawnObj.OnSpawnDone += CheckLevelPass;
                
            }
            
            propSpawnManger.PropSpawnData = ProjectVariables.Instance.LevelPropSpawnDatas[GameManager.Instance.level];
            propSpawnManger.BeginSpawn();
        }


        private int spawnDoneCount;
        /// <summary>
        ///CheckLevelPass 
        /// </summary>
        /// <returns></returns>
        private void CheckLevelPass()
        {
            spawnDoneCount++;
            if (spawnDoneCount == ProjectVariables.Instance.LevelEnemySpawnSettings[GameManager.Instance.level].settings.Count)
            {
                PassLevel();
            }
        }

        /// <summary>
        ///通过此level关之后的过程    触发方式
        /// 1. 显示bonus提示
        /// 2.1 进入ability choose scene,取消player input controller
        /// 2.2 （这是最后一贯） 显示胜利bonus
        /// </summary>
        public void PassLevel()
        {
            //some work
            DestroyAbilityPanel();
            Destroy(player.gameObject.GetComponent<PlayerHpBar>());
            if (GameManager.Instance.level+1  == ProjectVariables.Instance.LevelEnemySpawnSettings.Count)
            {
                // Debug.Log("Congratulations! you pass the game");
                StartCoroutine(DisplayGamePassBanner());
                StartCoroutine(GoToStartScene());
                return;
            }
            else
            {
                StartCoroutine(DisplayLevelPassBanner(GameManager.Instance.level ++));
                StartCoroutine(GoToAbilityChooseScene());
            }
            
            
        }


        public void GoToStartSceneImmediately()
        {
            DestroyAbilityPanel();
            SceneManager.LoadScene("start");
        }
        private  IEnumerator GoToStartScene()
        {
            yield return new WaitForSeconds(BannerDisplayTime);
            GameManager.Instance.level = 0;
            SceneManager.LoadScene("start");
        }
        
        private  IEnumerator GoToAbilityChooseScene()
        {
            yield return new WaitForSeconds(BannerDisplayTime);
            SceneManager.LoadScene("ability choose");
        }
        
        public void PlayerDead()
        {

            StartCoroutine(DisplayLevelFailBanner());
            StartCoroutine(GoToStartScene());
        }

        #region 跳转
        public void ReEnterFightScene()
        {
            //TODO关卡信息
            SceneManager.LoadScene("fight");
        }

        #endregion


        #region info panel
        public void PlayDialog(string message)
        {
            this.StartCoroutine(PlayDialogProgress(message));
        }
        private IEnumerator PlayDialogProgress(string message)
        {
            dialogTxt.transform.parent.gameObject.SetActive(true);
            dialogTxt.text = message;
            yield return new WaitForSeconds(BannerDisplayTime);
            dialogTxt.transform.parent.gameObject.SetActive(false);
        }

        public IEnumerator DisplayLevelBeginBanner( )
        {
            levelBeginBanner.gameObject.SetActive(true);
            yield return new WaitForSeconds(BannerDisplayTime);
            levelBeginBanner.gameObject.SetActive(false);
        }
        public IEnumerator DisplayLevelPassBanner(int level )
        {
            levelPassBanner.gameObject.SetActive(true);
            levelPassBanner.GetComponent<TextMeshProUGUI>().text = $"Level {level} Pass!";
            yield return new WaitForSeconds(BannerDisplayTime);
            levelPassBanner.gameObject.SetActive(false);
        }
        
        public IEnumerator DisplayLevelFailBanner( )
        {
            levelFailBanner.gameObject.SetActive(true);
            yield return new WaitForSeconds(BannerDisplayTime);
            levelFailBanner.gameObject.SetActive(false);
        }

        public IEnumerator DisplayGamePassBanner( )
        {
            levelPassBanner.gameObject.SetActive(true);
            levelPassBanner.GetComponent<TextMeshProUGUI>().text = $"All Levels Pass! Congratulations!";
            yield return new WaitForSeconds(BannerDisplayTime);
            levelPassBanner.gameObject.SetActive(false);
        }
        
        public void ClearWeaponPanel()
        {
            for (int i = 0; i < weaponPanel.childCount; i++)//clear
            {
                Destroy(weaponPanel.GetChild(i).gameObject);
            }
        }

        public void GenWeaponPanel()
        {
            ClearWeaponPanel();
            var weapons=GlobalPlayerManger.Instance.Player.WeaponsController.weapons;
            foreach (var wp in weapons)
            {
                var obj = Instantiate(weaponIconPrefab, weaponPanel.transform);
                Sprite wpSp = null;
                if (wp is NormalWeapon)
                {
                    wpSp= NormalWeaponSprite;
                }else if (wp is BombWeapon)
                {
                    wpSp = BombWeaponSprite;
                }

                obj.GetComponent<Image>().sprite = wpSp;
            }
        }
        private void AddWeaponIcon(Weapon wp)
        {
            var obj = Instantiate(weaponIconPrefab, weaponPanel.transform);
            Sprite wpSp = null;
            if (wp is NormalWeapon)
            {
                wpSp= NormalWeaponSprite;
            }else if (wp is BombWeapon)
            {
                wpSp = BombWeaponSprite;
            }

            obj.GetComponent<Image>().sprite = wpSp;
        }
        public void GenAbilityPanel()
        {
            for (int i = 0; i < abilityPanel.childCount; i++)//clear
            {
                Destroy(abilityPanel.GetChild(i).gameObject);
            }
            var abilityEquips = GlobalPlayerManger.Instance.Player.GetAbilities();
            foreach (var ability in abilityEquips)
            {
                var obj = Instantiate(abilityIconPrefab, abilityPanel.transform);
                if (obj.TryGetComponent<AbilityIcon>(out var item))
                {
                    var texture = Resources.Load(ability.GetIntro().SpritePath) as Texture2D;
                    var sp=Sprite.Create(texture,new Rect(0, 0, texture.width, texture.height),Vector2.one);

                    item.SetIcon(sp);
                    ability.OnCoolDownCallback += item.SetFill;
                }
                else
                {
                    Debug.LogError("abilityIconPrefab is not a AbilityItem");
                }
                
            }
        }

        public void DestroyAbilityPanel()
        {
            var abilityEquips = GlobalPlayerManger.Instance.Player.GetAbilities();
            foreach (var ability in abilityEquips)
            {
                ability.ClearOnCoolDown();
            }
        }


        private void OnDestroy()
        {
            GlobalPlayerManger.Instance.Player.WeaponsController.weaponEquipEvent -= AddWeaponIcon;

            // DestroyAbilityPanel();
        }

        #endregion
    }
}