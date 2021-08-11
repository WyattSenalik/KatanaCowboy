using UnityEngine;

/// <summary>
/// When this is shot, it will destroy its kill object.
/// </summary>
public class KillShootable : Shootable
{
    // Object to kill when this dies
    [SerializeField] private GameObject _killObject = null;

    /// <summary>
    /// Override form Shootable.
    /// Destroys the kill object.
    /// </summary>
    protected override void Die()
    {
        if (_killObject != null)
        {
            Destroy(_killObject);
        }
    }
}
