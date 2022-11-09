using System;
using script.consts;
using UnityEngine;

namespace script.Spaceship.Part
{
    public class Weapon:MonoBehaviour
    {
        public GameObject bulletPrefab;
        //owner
        private Spaceship mOwner;

        public Spaceship owner
        {
            get
            {
                if (mOwner == null) mOwner = transform.GetComponentInParent<Spaceship>();
                return mOwner;
            }
            set => mOwner = value;
        }
        //声音 
        public AudioClip fireAudioClip;
        //声音源
        private AudioSource audio;

        public event Action<Weapon, Bullet> AfterInstantiateBullet;

        public void ManualAfterInstantiateBullet(Weapon wp, Bullet bulletInstance)
        {
            AfterInstantiateBullet?.Invoke(wp,bulletInstance);
        }
        
        protected AudioSource fireAudio
        {
            get
            {
                if (audio == null)
                    audio = GetComponent<AudioSource>();
                if (audio == null)
                    audio=gameObject.AddComponent<AudioSource>();
                return audio;
            }
        }
        

        protected Transform _transform;

        public void SetVolume(float val)
        {
            fireAudio.volume = val;
        }
        
        private void Start()
        {
            _transform = transform;
        }
        
        public virtual void Fire()
        {
            //
            //

         
        }
    }
}