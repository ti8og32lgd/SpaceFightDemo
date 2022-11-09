using System;
using script.Buffer;
using script.consts;
using script.Spaceship;
using UnityEngine;
using UnityEngine.UIElements;

namespace script.Weapons
{
    public class AutoAim : MonoBehaviour//TODO 改名为自动追踪
    {
        private string targetTag;
        private Transform targetTransform;
        private BufferValue speed;
        private float startTime;
        public float BesselFactor =>ProjectVariables.Instance.besselFactor;
        public void Start()
        {
            targetTag = GetComponent<Bullet>().ammoOwner == EntityType.Player ? Tags.Enemy : Tags.Player;
            targetTransform = GameObject.FindGameObjectWithTag(targetTag)?.transform;
            GetComponent<Bullet>().MTranslateUpdate = AutoAimTranslateUpdate;
            speed = GetComponent<Bullet>().speed;
            startTime = Time.time;
        }


        private float costTime;
        private Vector3 startPoint,controlPoint,endPoint;
        
        public void AutoAimTranslateUpdate()
        {
            if (targetTransform == null) //go find next
            {
                transform.Translate(transform.forward * Time.deltaTime * speed.GetResultValF());
                return;
            }

            if (costTime == 0f && (Time.time-startTime)>0.8f)                //init
            {
                startTime = Time.time;
                costTime = (targetTransform.position - transform.position).magnitude*BesselFactor / speed.GetResultValF();
                startPoint = transform.position;
                endPoint = targetTransform.position;
                controlPoint=(startPoint+ endPoint) / 2             //mid point
                            + Vector3.Cross((startPoint+ endPoint) / 2,ProjectVariables.Instance.fightPlaneNormal).normalized
                             *(targetTransform.position - transform.position).magnitude*0.5f;
                // Debug.Log($"{startPoint} {endPoint} {controlPoint} {costTime}");
                
            }

            if (costTime < Time.time - startTime)
            {
                transform.Translate(transform.forward * Time.deltaTime * speed.GetResultValF());
                return;
            }
            
            // var delta = speed.GetResultValF() * Time.deltaTime;

            //cost time: length/speed *factor
           
            // Debug.Log($"{(Time.time-startTime)/costTime}");
            var pos = Bessel(startPoint, controlPoint, endPoint, (Time.time-startTime)/costTime);
            transform.position = pos;
            transform.LookAt(targetTransform);
            //TODO  Lerp  -> 优美的曲线
        }

        public Vector3 Bessel(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }
    }
}