using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    public static T Instance;

    public MonoBehaviourSingleton()
    {
        if (Instance != null) return;

        Instance = (T)this;
    }
}
