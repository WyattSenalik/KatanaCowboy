/// <summary>
/// When this gameobject is shot, it will be destroyed.
/// </summary>
public class KillShootable : Shootable
{
    /// <summary>
    /// Override form Shootable.
    /// Destroys the current gameobject.
    /// </summary>
    protected override void Die()
    {
        Destroy(this.gameObject);
    }
}
