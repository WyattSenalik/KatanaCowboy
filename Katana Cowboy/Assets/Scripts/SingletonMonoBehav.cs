using UnityEngine;

/// <summary>
/// Base class for any class that wants to be a monobehavior singleton.
/// </summary>
/// <typeparam name="T">Must be the type of the class that inherits from this class.</typeparam>
public abstract class SingletonMonoBehav<T> : MonoBehaviour where T : SingletonMonoBehav<T>
{
    // Singleton
    private static T instance = null;
    public static T Instance {
        get
        {
            return instance;
        }
    }

    // Called 0th
    // Set references
    protected virtual void Awake()
    {
        // Set up the singleton
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Debug.LogError("Cannot have multiple InputControllers");
            Destroy(this);
        }
    }
}
