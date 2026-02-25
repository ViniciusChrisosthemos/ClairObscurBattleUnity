using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    [Header("Events")]
    public UnityEvent OnParryEvent;
    public UnityEvent OnEscapePressed;

    public void OnParry()
    {
        OnParryEvent?.Invoke();
    }

    public void OnEscape()
    {
        OnEscapePressed?.Invoke();
    }
}
