using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO play get appropriate animation and such when getting hit.
/// </summary>
public class CharacterHittable : MonoBehaviour, IHittable
{
    [SerializeField] private int _startingHealth = 5;
    private int _currentHealth = 0;


    private void Awake()
    {
        _currentHealth = _startingHealth;
    }


    public void Hit(Hit hit)
    {
        if (--_currentHealth <= 0)
        {
            // TODO temporarily just destroying the whole gameobject
            Destroy(gameObject);
        }
    }
}
