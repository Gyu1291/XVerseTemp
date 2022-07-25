using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NetState : NetworkBehaviour
{

    // current movement state of the character
    // this is NOT a syncVar! it is synced by script,
    // because its change is coupled to MovementRequest, and syncing this is thus *redundant*.
    public MovementState MovementState = MovementState.Idle;
   


    /// <summary>
    /// Client should tell the server where it wants to go.
    /// Use the Vector3 variable any way you want 
    /// - send just the direction to the server and have the server calculates the destination..
    /// - send the target position of the character..
    /// </summary>
    public event Action<Vector3> MovementRequestReceived;

    [Command]
    public void MovementRequestServerRpc(Vector3 targetPos)
    {
        MovementRequestReceived?.Invoke(targetPos);
    }






    public event Action<MovementState> MovementChangeReceivedClient;
    /// <summary>
    /// movement requires some events to be broadcasted to the client,
    /// besides the everyday transform and rotation stuff - these are taken care of by special Mirror components.
    /// </summary>
    /// <param name="state"> the new updated MovementState </param>
    [ClientRpc]
    public void MovementChangeClientRPC(uint state)
    {
        MovementChangeReceivedClient?.Invoke((MovementState)state);
    }

    public bool IsMoving
    {
        get => MovementState == MovementState.Walking;
    }

    public bool IsLocalPlayer
    {
        get => isLocalPlayer;
    }
}

// ForcedMovement means that the character is being 'animated', not controlled by player.
// for example, climbing a ladder.
public enum MovementState : uint
{
    Idle,
    Walking,
    ForcedMovement
}