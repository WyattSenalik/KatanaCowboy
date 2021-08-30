using UnityEngine;

/// <summary>
/// Controls when the swords starts and stops attacking.
/// </summary>
public class SwordController : MonoBehaviour
{
    /// <summary> Reference to the character's animator. </summary>
    [SerializeField] private CharacterAnimator charAnim = null;

    public bool IsAttacking => isAttacking;
    private bool isAttacking = false;

    /// <summary>
    /// Called by third person movement script to begin the swing animation of the sword.
    /// </summary>
    public void StartSwingAnimation()
    {
        isAttacking = true;
        charAnim.StartAttackAnimation();
    }
    /// <summary>
    /// Stops attacking.
    /// Called from the attack animation.
    /// </summary>
    public void StopSwingAnimation()
    {
        isAttacking = false;
        charAnim.StopAttackAnimation();
    }
}
