using UnityEngine;

public abstract class Shootable : MonoBehaviour
{
    // Name of the layer that can be shot.
    public static readonly string SHOOT_LAYER_NAME = "Shootable";

    // Customizables.
    // Maximum health the shootable has.
    [SerializeField]
    protected float maxHealth = 1f;

    // The current health of the shootable.
    protected float curHealth;
    // The raycast hit information of the shot.
    protected ShotInfo shotInfo;

    // Called before the first frame.
    private void Start()
    {
        // If we are changing the layer from some other specified layer, let me know.
        if (this.gameObject.layer != LayerMask.NameToLayer("Default") &&
            this.gameObject.layer != LayerMask.NameToLayer(SHOOT_LAYER_NAME))
        {
            Debug.Log("Changed " + this.gameObject.name + "'s layer from " +
                LayerMask.LayerToName(this.gameObject.layer) + " to " + SHOOT_LAYER_NAME);
        }
        // Change the layer of the shootable.
        this.gameObject.layer = LayerMask.NameToLayer(SHOOT_LAYER_NAME);

        // Initialize health.
        curHealth = maxHealth;
    }

    /// <summary>
    /// Reduces the shootables health by the given damage.
    /// If reduced to zero or below, calls die.
    /// </summary>
    /// <param name="_damageToTake_">Damage the object will take from the shot.</param>
    /// <param name="_shotInfo_">Information about the shot.</param>
    public void GetShot(float _damageToTake_, ShotInfo _shotInfo_)
    {
        shotInfo = _shotInfo_;

        curHealth -= _damageToTake_;
        if (curHealth <= 0f)
        {
            curHealth = 0f;
            Die();
        }
    }

    /// <summary>
    /// Handles what happens when the game object reaches or falls below 0 hp.
    /// </summary>
    protected abstract void Die();
}
