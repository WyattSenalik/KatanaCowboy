using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectInfinitly : MonoBehaviour
{
    // The speed the object should move.
    [SerializeField]private float speed = 1f;
    // The direction the object should move locally.
    [SerializeField] private Vector3 localMoveDirection = Vector3.up; 


    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(MoveCoroutine());   
    }

    /// <summary>
    /// Moves the object in the specified direction.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveCoroutine()
    {
        // Continuously update the position of the object.
        while (true)
        {
            this.transform.position += this.transform.rotation * localMoveDirection * speed * Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Sets the local direction to move the object in.
    /// </summary>
    /// <param name="_moveDir_">Direction to move the object in. Should be normalized.</param>
    public void SetMoveDirection(Vector3 _moveDir_)
    {
        localMoveDirection = _moveDir_;
    }
}
