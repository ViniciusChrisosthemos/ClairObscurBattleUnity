using UnityEngine;

public class DestroySafelyController : MonoBehaviour
{
    [SerializeField] private float m_delay = 0.5f;

    public void TriggerDestrouSafely()
    {
        Destroy(gameObject, m_delay);
    }
}
