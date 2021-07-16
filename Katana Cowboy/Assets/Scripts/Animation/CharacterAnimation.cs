using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterAnimation : MonoBehaviour
{
    // Constants
    // Animator Parameter Names
    private const string ANIM_VARNAME_FLOAT_HORI_MOVE = "MoveHorizontal";
    private const string ANIM_VARNAME_FLOAT_VERT_MOVE = "MoveVertical";
    private const string ANIM_VARNAME_BOOL_IS_MOVING = "IsMoving";

    [SerializeField] private Animator _anim = null;
    [SerializeField] private float _smoothSpeed = 0.1f;

    private CharacterMovement _charMoveRef = null;


    private void Awake()
    {
        _charMoveRef = GetComponent<CharacterMovement>();
    }
    private void Update()
    {
        ApplyMovementAnimation();
    }


    private bool CheckIfMoving()
    {
        // Get the character's actual movement direction
        Vector3 moveDir = _charMoveRef.MovementDirection;
        return moveDir.sqrMagnitude > 0.01f;
    }
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
