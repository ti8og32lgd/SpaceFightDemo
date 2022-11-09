using System;
using System.Collections;
using UnityEngine;

namespace script.UI
{
    public class HpChangeIndicator : MonoBehaviour
    {
        public Vector3 moveDirection;
        public float exitTime = 1.5f;
        public float moveSpeed = 0.3f;
        private void Start()
        {
            StartCoroutine(LagDestroySelf());
        }

        private IEnumerator LagDestroySelf()
        {
            yield return new WaitForSeconds(exitTime);
            Destroy(gameObject);
        }
        private void Update()
        {
            transform.Translate(moveDirection*moveSpeed*Time.deltaTime,Space.Self);
        }
    }
}