using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class VFXAnimationHelper : MonoBehaviour, IMutableAnimationSpeed
{
    [SerializeField] private bool m_autoPlay = true;
    [SerializeField] private List<ParticleSystem> m_particleSystems;

    private void Update()
    {
        if (m_autoPlay)
        {
            SetAnimationSpeed(0);
        }
    }

    public void SetAnimationSpeed(float speed)
    {
        foreach (ParticleSystem particle in m_particleSystems)
        {
            particle.Simulate(TimeManager.Instance.DeltaTime, true, false);
        }
    }

    public void SetAutoPlay(bool autoPlay)
    {
        m_autoPlay = autoPlay;
    }
}
