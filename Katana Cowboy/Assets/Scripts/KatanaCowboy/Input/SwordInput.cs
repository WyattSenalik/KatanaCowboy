using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class SwordInput : MonoBehaviour
{
    public event Action<bool> onAttack;

    public bool attack { get; private set; }


    private void OnAttack(InputValue value)
    {
        Attack(value.isPressed);
    }

    private void Attack(bool newAttackState)
    {
        attack = newAttackState;
        onAttack?.Invoke(attack);
    }
}
