using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterAnimation : MonoBehaviour
{
    // Constants
    // Animator Parameter Names
    private const string ANIM_VARNAME_SPEED = "MoveSpeed";

    [SerializeField] private Animator _anim = null;

    private CharacterMovement _charMoveRef = null;


    private void Awake()
    {
        _charMoveRef = GetComponent<CharacterMovement>();
    }
    private void Update()
    {
        float speed = _charMoveRef.CurrentSpeed / _charMoveRef.RunSpeed;
        _anim.SetFloat(ANIM_VARNAME_SPEED, speed);
    }
}
