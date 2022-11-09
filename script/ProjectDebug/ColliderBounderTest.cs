using UnityEngine;

namespace script.ProjectDebug
{
    public class ColliderBounderTest : MonoBehaviour
    {
        private void OnTriggerExit(UnityEngine.Collider other)
        {
            Debug.Log($"{name} exit {other.name}");
            Destroy(other.gameObject);
        }
    }
}