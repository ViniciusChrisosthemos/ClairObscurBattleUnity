using System;
using UnityEngine;

public class DestroySafelyController : MonoBehaviour
{
    [SerializeField] private float m_delay = 0.5f;
    [SerializeField] private bool m_autoDestroction = false;

    private void Start()
    {
        if (m_autoDestroction) TriggerDestrouSafely();
    }

    public void TriggerDestrouSafely()
    {
        Destroy(gameObject, m_delay);
    }

    public void SetAutoDestry(bool autoDestroy)
    {
        m_autoDestroction = autoDestroy;
    }
}
