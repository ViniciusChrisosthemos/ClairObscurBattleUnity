using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindFirstObjectByType<T>();

            return _Instance;
        }
    }
}