using script.consts;
using UnityEngine;

namespace script
{
    public class AirBarrier : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                return;
            }

            if (other.CompareTag(Tags.Shield))
            {
                return;
            }
            Debug.Log($"Hit Air Barrier And Be Destroyed: {other.name} ");
            Destroy(other.gameObject);
        }
    }
}