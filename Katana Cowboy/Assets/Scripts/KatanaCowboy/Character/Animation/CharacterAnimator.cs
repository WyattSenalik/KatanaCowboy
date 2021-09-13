using UnityEngine;

/// <summary>
/// Controls the animations of a character.
/// </summary>
[RequireComponent(typeof(CharacterMovement))]
public class CharacterAnimator : MonoBehaviour
{
    // Constants
    // Animator Parameter Names
    private const string ANIM_VARNAME_FLOAT_HORI_MOVE = "MoveHorizontal";
    private const string ANIM_VARNAME_FLOAT_VERT_MOVE = "MoveVertical";
    private const string ANIM_VARNAME_BOOL_IS_ATTACKING = "Attack";


    // Reference to the animator this controls
    [SerializeField] private Animator _anim = null;
    // Lerp speed
    [SerializeField] private float _smoothSpeed = 0.1f;

    // Reference to the character movement script to pull movement info from
    private CharacterMovement _charMoveRef = null;


    #region UnityMessages
    // Called 0th
    // Domestic Initialization
    private void Awake()
    {
        _charMoveRef = GetComponent<CharacterMovement>();
    }
    // Called once every frame
    private void Update()
    {
        ApplyMovementAnimation();
    }
    #endregion UnityMessages


    /// <summary>
    /// Starts the attacking animation.
    /// </summary>
    public void StartAttackAnimation()
    {
        _anim.SetBool(ANIM_VARNAME_BOOL_IS_ATTACKING, true);
    }
    /// <summary>
    /// Stops the attacking animation.
    /// </summary>
    public void StopAttackAnimation()
    {
        _anim.SetBool(ANIM_VARNAME_BOOL_IS_ATTACKING, false);
    }


    /// <summary>
    /// Updates the animator variables based on the character's current movement.
    /// </summary>
    private void ApplyMovementAnimation()
    {
        // Get the character's actual movement direction
        Vector3 moveDir = _charMoveRef.MovementDirection;
        // Basis vectors
        Vector3 right = _charMoveRef.transform.right;
        Vector3 forward = _charMoveRef.transform.forward;
        // Convert the actual movement direction to be in terms of the local basis
        float hori = Vector3.Dot(moveDir, right);
        float vert = Vector3.Dot(moveDir, forward);

        // Get the previous values
        float prevHori = _anim.GetFloat(ANIM_VARNAME_FLOAT_HORI_MOVE);
        float prevVert = _anim.GetFloat(ANIM_VARNAME_FLOAT_VERT_MOVE);
        // Smooth the new values using the previous values
        hori = Mathf.Lerp(prevHori, hori, _smoothSpeed);
        vert = Mathf.Lerp(prevVert, vert, _smoothSpeed);

        // Apply the horizontal and vertical
        _anim.SetFloat(ANIM_VARNAME_FLOAT_HORI_MOVE, hori);
        _anim.SetFloat(ANIM_VARNAME_FLOAT_VERT_MOVE, vert);
    }
}
