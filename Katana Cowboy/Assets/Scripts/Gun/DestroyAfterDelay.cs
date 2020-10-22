using System.Collections;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    // The amount of seconds to wait before the object is destroyed.
    [SerializeField]
    private float secondsToWait = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        // Start the death coroutine.
        StartCoroutine(DeathCoroutine());
    }

    /// <summary>
    /// Coroutine.
    /// Waits the specified amount of seconds and then destroys this object.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(secondsToWait);
        Destroy(this.gameObject);
        yield return null;
    }
}
