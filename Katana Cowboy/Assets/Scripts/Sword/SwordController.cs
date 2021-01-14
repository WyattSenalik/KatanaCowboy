using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    /// <summary> Reference to the animator on the sword. </summary>
    [SerializeField]
    private Animator swordAnim = null;
    /// <summary> Reference tot he third person controller script on the player. </summary>
    [SerializeField]
    private ThirdPersonMovement playerMoveRef = null;

    /// <summary>
    /// Called by third person movement script to begin the swing animation of the sword.
    /// </summary>
    public void StartSwingAnimation()
    {
        swordAnim.SetBool("isAttacking", true);
    }

    /// <summary>
    /// Called by the swing animation to let us know its over.
    /// Aler the player movement script that we are no longer attacking.
    /// </summary>
    public void StopSwingAnimation()
    {
        swordAnim.SetBool("isAttacking", false);
        playerMoveRef.FinishAttack();
    }
}
