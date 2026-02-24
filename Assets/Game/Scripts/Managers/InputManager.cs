using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    [Header("Events")]
    public UnityEvent OnParryEvent;

    public void OnParry()
    {
        OnParryEvent?.Invoke();
    }
}
