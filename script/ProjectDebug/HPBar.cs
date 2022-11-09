using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace script.ProjectDebug
{
    /// <summary>
    ///  简易显示血量，测试用
    /// </summary>
    public class HPBar : MonoBehaviour
    {
        public TMP_Text txtPrefab,txt;
        private Spaceship.Spaceship sp;
        private Vector3 offset;
        private void Start()
        {
            txt = Instantiate(txtPrefab);
            txt.transform.SetParent(FindObjectOfType<Canvas>().transform);
            sp = GetComponent<Spaceship.Spaceship>();
            offset=GetComponentInChildren<MeshFilter>().mesh.bounds.max;
        }

        private void Update()
        {
            if (sp == null)
            {
                Destroy(gameObject);
            }
            txt.text = sp.nowHp.ToString();
            txt.transform.position = Camera.main.WorldToScreenPoint(sp.transform.position+offset) ;

        }
    }
}