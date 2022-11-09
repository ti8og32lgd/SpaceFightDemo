using System;
using UnityEngine;

namespace script.ProjectDebug
{
    public class ColliderTest : MonoBehaviour
    {
        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            Debug.Log($"{name} enter {other.name}");
        }
    }
}