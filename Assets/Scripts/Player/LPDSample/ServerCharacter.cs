using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCharacter : NetworkBehaviour
{
    public ServerCharacterMovement MovementProvider;
    public NetState NetState;

    // we know that Mirror only enables the gameObject after the connection is ready.
    // we can be sure that the variables like isServer, isClient are ready when Start executes.
    private void Start()
    {
        NetState = GetComponent<NetState>();

        // initialize MovementProvider and make initial state sync.
        MovementProvider = GetComponent<ServerCharacterMovement>();
        if (MovementProvider.MovementState != NetState.MovementState) NetState.MovementState = MovementProvider.MovementState;

        if (!NetState.isServer)
        {
            // disable the movement provider. this is important!!
            MovementProvider.enabled = false;
            this.enabled = false;
        }
        else
        {
            NetState.MovementRequestReceived += OnMovementRequest;
        }
    }






    // movement process loop.
    private void FixedUpdate()
    {
        // state update.
        if (NetState.MovementState != MovementProvider.MovementState)
        {
            // change movement state.
            Debug.Log($"[ServerCharacter] : move from {NetState.MovementState} >> {MovementProvider.MovementState} ");
            NetState.MovementState = MovementProvider.MovementState;
            NetState.MovementChangeClientRPC((uint)MovementProvider.MovementState);
        }
    }

    // this runs on the server only.
    private void OnMovementRequest(Vector3 targetPosition)
    {
        MovementProvider.MoveTo(targetPosition);
    }


}
