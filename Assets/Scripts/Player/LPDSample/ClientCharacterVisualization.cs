using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCharacterVisualization : MonoBehaviour
{
    // any visual components, animator, sound player, particles,, should be set here.
    // you must make them accessible(i.e. public)

    public Animator Anim;


    private void Start()
    {
        Anim = GetComponent<Animator>();

        // this in fact should go in OnSpawn callback.
        NetState netState = GetComponentInParent<NetState>();
        if (!netState.isClient) return;

        netState.MovementChangeReceivedClient += OnMovementChange;
    }

    // CALLBACKS
    private void OnMovementChange(MovementState state)
    {
        if (state == MovementState.Walking) Anim.SetBool("Walk", true);
        else if (state == MovementState.Idle) Anim.SetBool("Walk", false);
    }


}
