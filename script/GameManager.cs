using System;
using script.Scene;
using script.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace script
{
    public class GameManager:Singleton<GameManager>
    {
        public int level=0;//当前关卡


        public void Start()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
        }

        public static void Gameover()
        {
            var fightSceneController = FindObjectOfType<FightScene>();
            fightSceneController.PlayerDead();
        }
    }
}