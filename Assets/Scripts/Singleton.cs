using JetBrains.Annotations;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    public static T Instance { get; private set; }

    protected virtual bool dontDestroyOnLoad
    {
        get { return false; }
    }

[UsedImplicitly]
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = (T) this;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
    }

    [UsedImplicitly]
    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
