using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public float TimeScale { get; private set; } = 1f;

    public void SetTimeScale(float scale)
    {
        TimeScale = scale;
    }

    public float DeltaTime => Time.deltaTime * TimeScale;
}