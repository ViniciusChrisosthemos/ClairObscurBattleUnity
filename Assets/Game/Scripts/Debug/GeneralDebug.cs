using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralDebug : MonoBehaviour
{
    [SerializeField] private QuickTimeEventManager m_quickTimeEventManager;
    [SerializeField] private int m_amount;
    [SerializeField] private float m_interval;
    [SerializeField] private float m_qteTime;

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            m_quickTimeEventManager.StartEvents(m_qteTime, m_amount, m_interval);
        }
    }
}
