using TMPro;
using UnityEngine;

namespace script.UI
{
    public class HpChangeDisplay : MonoBehaviour
    {
        public TMP_Text indicatorPrefab;
        public RectTransform displayCanvas;

        public void Indicate(int value, Color color, Vector3 worldPos, float scaleFactor = 1f)
        {
            var obj = Instantiate(indicatorPrefab, displayCanvas);
            obj.text = (value > 0 ? "+" : "") + value.ToString();
            obj.color = color;
            var screenPos = Camera.main!.WorldToScreenPoint(worldPos);
            if (
                RectTransformUtility.ScreenPointToLocalPointInRectangle(displayCanvas, screenPos, null,
                    out var localPoint)
            )
            {
                // Debug.Log(localPoint);
                // obj.GetComponent<RectTransform>().anchoredPosition = localPoint;
                obj.GetComponent<RectTransform>().localPosition = localPoint;
                obj.GetComponent<RectTransform>().sizeDelta *= scaleFactor;
            }
        }
    }
}