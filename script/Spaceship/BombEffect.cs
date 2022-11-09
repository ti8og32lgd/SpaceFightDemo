using System;
using UnityEngine;

namespace script.Spaceship
{
    public class BombEffect : MonoBehaviour
    {

        public int explodeSpeed = 1;
        public Vector3 originScale ;
        private void Start()
        {
            originScale = transform.localScale;
            transform.localScale = Vector3.zero;
            
        }

        
        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originScale, explodeSpeed * Time.deltaTime);
            if ((transform.localScale - originScale).magnitude < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}