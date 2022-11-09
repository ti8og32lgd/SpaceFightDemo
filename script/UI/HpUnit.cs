using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace script.UI
{
    public class HpUnit : MonoBehaviour
    {
        private GameObject mChild;

        private Vector3 originalPos;
        [SerializeField]private float originalHeight, originalWidth;
        private Color originalColor;
        [SerializeField] private float phase1Speed = 5.0f;
        [SerializeField]private float phase2Speed = 15.0f;
        [SerializeField]float fill=1f;
        [SerializeField]float widthAmplifyScale=1.5f;
        [SerializeField]float heightAmplifyScale = 2.0f;
        [SerializeField]Color destColor = Color.red;

        private RectTransform rectTransform,childRectTransform;

        private void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
            childRectTransform = Child.GetComponent<RectTransform>();
        }


        

        private GameObject Child
        {
            get
            {
                if (mChild == null)
                {
                    mChild = transform.GetChild(0).gameObject;
                }
                return mChild;
            }
        }
        

        public void DisplayLose()
        {
            // Animator.SetTrigger("lose");
            //手动实现动画
            StartCoroutine(PlayLoseAnim());
        }
        
        
        private IEnumerator PlayLoseAnim()
        {
            
            var destWidth = originalWidth*widthAmplifyScale;
            var destHeight = originalHeight*heightAmplifyScale;
            var destSizeDelta = new Vector2(destWidth, destHeight);
            
            var t = 0f;

            while (true)
            {
                t += Time.deltaTime * phase1Speed;

                var newX=Mathf.Lerp(originalWidth, destWidth, t);
                var newY=Mathf.Lerp(originalWidth, destHeight, t);
                var newColor = Color.Lerp(originalColor,destColor, t);
                var newSizeDelta = new Vector2(newX, newY);
                
                rectTransform.sizeDelta = newSizeDelta;
                childRectTransform.sizeDelta=newSizeDelta;
                Child.GetComponent<Image>().color = newColor;

                if (( rectTransform.sizeDelta  - destSizeDelta).magnitude < 0.1f)//threshold
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            
            
            //to transparent
            t = 0f;
             while (true)
             {
                 t += Time.deltaTime * phase2Speed;
                 var newColor = Child.GetComponent<Image>().color;
                 newColor.a = Mathf.Lerp(1f, 0f, t);
                 Child.GetComponent<Image>().color = newColor;
                 if (Child.GetComponent<Image>().color.a < 0.01f)
                 {
                     break;
                 }
                 yield return new WaitForEndOfFrame();
             }
             gameObject.SetActive(false);

            yield break;
        }
        

        public void SetPositionAndSize(Vector3 localPosition,float height,float width)
        {
            // var rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition = localPosition;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,height);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,width);

            // var childRectTransform = Child.GetComponent<RectTransform>();//set child same as this
            childRectTransform.localPosition = Vector3.zero;
            childRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,height);
            childRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,width);

            originalHeight = height;
            originalPos = localPosition;
            originalWidth = width;
        }

        public void SetColor(Color color)
        {
            Child.GetComponent<Image>().color =color;
            originalColor = color;
        }
        
        public void ResetRect()
        {
            // Debug.Log(originalHeight+" "+originalWidth);
            // var rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition = originalPos;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,originalHeight);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,originalWidth);

            // var childRectTransform = Child.GetComponent<RectTransform>();//set child same as this
            childRectTransform.localPosition = Vector3.zero;
            childRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,originalHeight);
            childRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,originalWidth);
            
            Child.GetComponent<Image>().color = originalColor;
            
            //fill
            SetFill(1.0f);
        }
        
        public void SetFill(float rate)
        {
            // Debug.Log(rate);
            // var childRect = Child.GetComponent<RectTransform>();//set child same as this
            childRectTransform.localPosition = new Vector3(-originalWidth*(1-rate),0,0);
        }
    }
}