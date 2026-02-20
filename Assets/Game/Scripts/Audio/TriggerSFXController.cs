using UnityEngine;

public class TriggerSFXController : MonoBehaviour
{
    [SerializeField] private float m_volume;

    public void TriggerSFX(AudioClip audioClip)
    {
        SoundManager.Instance.PlaySFX(audioClip, m_volume);
    }
}
