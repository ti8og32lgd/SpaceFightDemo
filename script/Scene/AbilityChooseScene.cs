using System;
using System.Resources;
using script.Spaceship.Ability;
using script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace script.Scene
{
    public class AbilityChooseScene : MonoBehaviour
    {
        public PlayableDirector arriveDirector, leaveDirector;
        public Canvas selectCanvas;
        public RectTransform optionsGroup,alreadyHaveGroup;


        public GameObject abilityIconPrefab;
        
        
        public int optionCount=3;
        private void Start()
        {
            GlobalPlayerManger.Instance.ReplacePlayerInSceneWithGlobal();
            GlobalPlayerManger.Instance.Player.DisableInput();
            selectCanvas.gameObject.SetActive(false);

            arriveDirector.stopped += ArriveAnimDone;
            leaveDirector.stopped += LoadFightScene;
        }

        public void SkipFightScene()
        {
            LoadFightScene(null);
        }
        private void LoadFightScene(PlayableDirector obj)
        {
            GlobalPlayerManger.Instance.Player.transform.parent = null;

            DontDestroyOnLoad( GlobalPlayerManger.Instance.Player.gameObject);
            SceneManager.LoadScene("fight");
            // SceneManager.LoadScene("fight");
        }

        private void ArriveAnimDone(PlayableDirector obj)
        {
            ShowAbilitySelectCanvas();
        }

        private void SelectDone()
        {
            selectCanvas.gameObject.SetActive(false);
             PlayLeaveAnim();
        }
        private void PlayLeaveAnim()
        {
            leaveDirector.Play();
        }
        
        public void ShowAbilitySelectCanvas()
        {
            //1.enable
            selectCanvas.gameObject.SetActive(true);
            ClearOptionGroups();//clear

            //2. 生成3个待选的技能（exclude现有的）
            var infos=AbilityContainer.Instance.GetUnusedAbilityEquipInfo(optionCount);
            //3. 根据3个待选技能生成button（图片，说明，事件）
            var AbilityUnitPrefab = ProjectVariables.Instance.abilityUIItemPrefab;
            for (int i = 0; i < infos.Count; i++)
            {
                var obj = Instantiate(AbilityUnitPrefab, optionsGroup);
                if (obj.TryGetComponent<AbilitySelectItem>(out var item))
                {
                    var texture = Resources.Load(infos[i].Item2.SpritePath) as Texture2D;
                    var sp=Sprite.Create(texture,new Rect(0, 0, texture.width, texture.height),Vector2.one);
                    Debug.Log($"Try get sprite {infos[i].Item2.SpritePath},res:{texture }  sp:{sp}");
                    item.SetItem(infos[i].Item2.ChineseName,infos[i].Item2.ChineseIntro,sp);
                    
                }
                var i1 = i;
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    var abilityEquip= AbilityContainer.Instance.UseAbilityEquip(infos[i1].Item1);
                    GlobalPlayerManger.Instance.Player.EquipSpaceshipAbility(abilityEquip);
                    // abilityEquip.Do(playerRef);
                    SelectDone();
                });
            }
            
            //4.生成现有的技能
            for (int i = 0; i < alreadyHaveGroup.childCount; i++)//clear
            {
                Destroy(alreadyHaveGroup.GetChild(i).gameObject);
            }
            var abilityEquips = GlobalPlayerManger.Instance.Player.GetAbilities();
            foreach (var ability in abilityEquips)
            {
                var obj = Instantiate(abilityIconPrefab, alreadyHaveGroup.transform);
                if (obj.TryGetComponent<AbilityIcon>(out var item))
                {
                    var texture = Resources.Load(ability.GetIntro().SpritePath) as Texture2D;
                    var sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one);
                    item.SetIcon(sp);
                }
                else
                {
                    Debug.LogError("abilityIconPrefab is not a AbilityItem");
                }
            }
        }

        private void ClearOptionGroups()
        {
            //clear optionsGroup
            for (int i = 0; i < optionsGroup.childCount; i++)
            {
                Destroy(optionsGroup.GetChild(i).gameObject);
            }
        }
    }
}