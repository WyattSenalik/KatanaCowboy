using UnityEngine;

using StarterAssets;

/// <summary>
/// Controls when the swords starts and stops attacking.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SwordInput))]
[RequireComponent(typeof(ThirdPersonController))]
public class SwordController : MonoBehaviour
{
    private Animator _animator = null;
    private SwordInput _input = null;
    private ThirdPersonController _thirdPersonController = null;

    private int _animIDAttack = Animator.StringToHash("Attack");

    public bool isAttacking { get; private set; }


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<SwordInput>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
    }
    private void OnEnable()
    {
        _input.onAttack += Attack;
    }
    private void OnDisable()
    {
        _input.onAttack -= Attack;
    }


    /// <summary>
    /// Called by third person movement script to begin the swing animation of the sword.
    /// </summary>
    public void StartSwingAnimation()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            _animator.SetTrigger(_animIDAttack);
            _thirdPersonController.CanMove = false;
        }
    }
    /// <summary>
    /// Stops attacking.
    /// Called from the attack animation.
    /// </summary>
    public void StopSwingAnimation()
    {
        isAttacking = false;
        _thirdPersonController.CanMove = true;
    }


    private void Attack(bool attackState)
    {
        if (attackState)
        {
            StartSwingAnimation();
        }
    }
}
