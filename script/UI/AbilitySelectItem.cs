using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace script.UI
{
    public class AbilitySelectItem : MonoBehaviour
    {
        [Header("binding")] public TextMeshProUGUI abilityName;
        public TextMeshProUGUI abilityIntro;
        public Image abilityImage;

        public void SetItem(string name, string intro, Sprite sp)
        {
            abilityName!.text = name;
            abilityIntro!.text = intro;
            abilityImage.sprite =sp;
        }
    }
}