using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraBlurController : MonoBehaviour
{
    [SerializeField] private Volume m_globalVolume;
    [SerializeField] private float m_foucsDistance;

    private DepthOfField m_dof;
    private bool m_toggleDof;

    public void SetBlur(bool isActive)
    {
        m_toggleDof = isActive;

        SetBlur();
    }

    public void ToggleBackgroundUI()
    {
        m_toggleDof = !m_toggleDof;

        SetBlur();
    }

    private void SetBlur()
    {
        if (m_globalVolume.profile.TryGet(out m_dof))
        {
            m_dof.active = m_toggleDof;
            m_dof.focusDistance.value = m_foucsDistance;
        }
    }
}
