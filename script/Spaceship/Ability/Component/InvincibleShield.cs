using System;
using script.Spaceship.Part;
using UnityEngine;

namespace script.Spaceship.Ability.Component
{
    public class InvincibleShield : MonoBehaviour
    {
        
        
        public float coolDown = 15.0f;
        public float workTime = 5f;

        public float coolDownTimer = 0f;
        public bool isInvincible;
        ///冷却时间更新的callback
        /// 参数：冷却时间准备完成度 range(0,1) 
        public event Action<float> OnCoolDownCallback;

        public void InvokeOnCoolDown(float f)
        {
            OnCoolDownCallback?.Invoke(Mathf.Clamp(f,0,1));
        }
        private void Update()
        {
            if (coolDownTimer < 0f || isInvincible)
            {
                return;
            }
            coolDownTimer -= Time.deltaTime;
            InvokeOnCoolDown(1-(coolDownTimer)/coolDown);
        }

        public void TryEnableInvincible()
        {
            // Debug.Log("Try Enable Invincible");
            if (coolDownTimer > 0f||isInvincible)
            {
                return;
            }
            isInvincible = true;

            transform.localScale =Vector3.one* 1.5f;
            GetComponent<Shield>().isDestroyBullet = true;
            GetComponent<MeshRenderer>().material = ProjectVariables.Instance.goldenShieldMaterial;
            Invoke(nameof(DisableInvincible),workTime);
            // Debug.Log("Enable Invincible");
        }
        
        public void DisableInvincible()
        {
            transform.localScale =Vector3.one;
            GetComponent<Shield>().isDestroyBullet = false;
            coolDownTimer = coolDown;
            GetComponent<MeshRenderer>().material =null;
            isInvincible = false;

        }
    }
}