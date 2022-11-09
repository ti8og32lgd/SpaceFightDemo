using UnityEngine;
using UnityEngine.UI;

namespace script.UI
{
    /// <summary>
    /// 战斗场景中右侧icon显示
    /// </summary>
    public class AbilityIcon : MonoBehaviour
    {
        [Header("binding")]
        public Image fullImage;
        public Image fillImage;

        public void SetIcon(Sprite sp)
        {
            fullImage.sprite = sp;
            fillImage.sprite = sp;
        }

        public void SetFill(float fill)
        {
            fillImage.fillAmount = fill;
        }
    }
}