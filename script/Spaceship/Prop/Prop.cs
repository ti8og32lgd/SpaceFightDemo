using System;
using script.consts;
using script.Scene;
using UnityEngine;

namespace script.Spaceship.Prop
{
    public class Prop : MonoBehaviour
    {
        public EntityType pickedOwnerType = EntityType.Player;

        public event Action<Spaceship> OnPick;

        public float moveSpeed = 1f;
        public float rotateSpeed = 25f;

        public bool isRotate = true;
        public virtual void Pick(Spaceship spaceship)
        {
            Debug.Log($"{spaceship.name} Get Prop {name}");
            OnPick?.Invoke(spaceship);

            Destroy(gameObject);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Spaceship>(out var spaceship))
            {
                return;
            }

            if (spaceship.entityType != pickedOwnerType)
            {
                return;
            }


            Pick(spaceship);
        }

        private void Update()
        {
            transform.Translate(ProjectVariables.Instance.fightPlaneForward*-1*moveSpeed*Time.deltaTime,Space.World);
         
            if(isRotate)
                transform.Rotate(Vector3.one*rotateSpeed*Time.deltaTime);
        }
    }
}