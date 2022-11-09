using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace script.Scene
{
    public class StartScene : MonoBehaviour
    {
        private void Start()
        {
            GlobalPlayerManger.Instance.Player.DisableInput();
        }

        public void LoadFightScene()
        {
            GlobalPlayerManger.Instance.InitPlayer();
            SceneManager.LoadScene("fight");
        }
    }
}