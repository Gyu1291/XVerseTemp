using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCharacterMovement : MonoBehaviour
{
    private Vector3 _targetPos;
    private float _distanceCheck = 0.05f;
    private float _walkSpeed = 1.00f;


    public MovementState MovementState = MovementState.Idle;

    // this runs for possibly a large number of characters.
    // making this part lean will really pay off in terms of performance!
    private bool _dontUpdate = true;
    private void FixedUpdate()
    {
        if (_dontUpdate) return;
        //main update loop. checking _
        if (Vector3.Magnitude(transform.position - _targetPos) < _distanceCheck)
        {
            MovementState = MovementState.Idle;
            Debug.Log($"[ServerCharMovement] : arrived! ");
            _dontUpdate = true;
        }
        else
        {
            // should move towards target
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _walkSpeed);
        }


    }

    public void MoveTo(Vector3 target)
    {
        Debug.Log($"[ServerCharMovement] : move from {transform.position} >> {target} ");
        _targetPos = target;
        transform.LookAt(_targetPos, Vector3.up);
        MovementState = MovementState.Walking;
        _dontUpdate = false;
    }
}
