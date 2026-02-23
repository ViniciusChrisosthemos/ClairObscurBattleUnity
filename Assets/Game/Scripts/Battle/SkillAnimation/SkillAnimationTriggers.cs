using UnityEngine;
using UnityEngine.Events;

public class SkillAnimationTriggers : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnAnimationStart;
    public UnityEvent OnAnimationEnd;
    public UnityEvent OnQTEStart;
    public UnityEvent OnQTEEnd;
    public UnityEvent OnDamageEvent;

    public void TriggerQTEStart()
    {
        OnQTEStart?.Invoke();
    }

    public void TriggerQTEEnd()
    {
        OnQTEEnd?.Invoke();
    }

    public void TriggerAnimationStart()
    {
        OnAnimationStart?.Invoke();
    }

    public void TriggerAnimationEnd()
    {
        OnAnimationEnd?.Invoke();
    }

    public void TriggerDamageEvent()
    {
        OnDamageEvent?.Invoke();
    }
}
