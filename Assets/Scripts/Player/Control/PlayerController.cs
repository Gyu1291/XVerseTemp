using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

using XVerse.Player.Input;

namespace XVerse.Player.Control
{
    [RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody), typeof(NetworkTransform))]
    public abstract class PlayerController : NetworkBehaviour
    {
        [Header("Input Setting")]
        public InputSetting InputSetting;

        protected bool isEnabled = false;

        protected bool isValid => isLocalPlayer && isEnabled;

        protected void OnValidate()
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<NetworkTransform>().clientAuthority = true;
        }

        public override void OnStartLocalPlayer()
        {
            isEnabled = true;
        }

        public void SetInputSetting(InputSetting inputSetting)
        {
            InputSetting = inputSetting;
        }
    }
}
