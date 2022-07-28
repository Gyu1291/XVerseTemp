using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

using XVerse.Player.Control;

namespace XVerse.Player.Example
{
    public class PlayerControllerExample : PlayerController
    {
        private const float SPEED_MIN = 2f;
        private const float SPEED_MAX = 10f;
        private const float JUMP_DELAY = 0.3f;

        public enum STATE
        {
            NONE = -1,
            MOVE = 0,
            JUMP,
            FLOAT,
            STOP,
        }

        [Header("Movement Settings")]
        [Range(SPEED_MIN, SPEED_MAX)]
        public float WalkSpeed = 4f;
        [Range(SPEED_MIN, SPEED_MAX)]
        public float RunSpeed = 7f;
        public float JumpForce = 3f;
        public float TurnSensitivity = 5f;

        [Header("Diagnostics")]
        public STATE State = STATE.NONE;
        public bool IsGrounded = true;
        public bool CanJump = true;
        public float MoveSpeed;
        public int Horizontal;
        public int Vertical;

        private Vector3 velocity;

        private void Start()
        {
            reset();
        }

        [ClientCallback]
        private void Update()
        {
            if (!isValid) return;

            if (IsGrounded)
            {
                if (InputSetting.KeyInput("Movement", "Front") || InputSetting.KeyInput("Movement", "Left") || InputSetting.KeyInput("Movement", "Right") || InputSetting.KeyInput("Movement", "Back"))
                {
                    State = STATE.MOVE;
                    if (InputSetting.KeyInput("Movement", "Run")) { MoveSpeed = RunSpeed; }
                    else if (MoveSpeed != WalkSpeed) { MoveSpeed = WalkSpeed; }
                }
                else if (velocity.magnitude < 0.1f) { State = STATE.STOP; }
                if (CanJump && InputSetting.KeyInput("Movement", "Jump"))
                {
                    State = STATE.JUMP;
                    GetComponent<Rigidbody>().AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                    InputSetting.InputLockAll();
                    CanJump = false;
                    IsGrounded = false;
                }
            }
            else
            {
                State = STATE.FLOAT;
            }

        }

        [ClientCallback]
        private void FixedUpdate()
        {
            if (!isValid) return;

            if (State == STATE.MOVE)
            {
                if (InputSetting.KeyInput("Movement", "Front")) { Vertical = 1; }
                if (InputSetting.KeyInput("Movement", "Back")) { Vertical = -1; }
                if (InputSetting.KeyInput("Movement", "Front") && InputSetting.KeyInput("Movement", "Back")) { Vertical = 0; }
                if (!InputSetting.KeyInput("Movement", "Front") && !InputSetting.KeyInput("Movement", "Back")) { Vertical = 0; }

                if (InputSetting.KeyInput("Movement", "Left")) { Horizontal = 1; }
                if (InputSetting.KeyInput("Movement", "Right")) { Horizontal = -1; }
                if (InputSetting.KeyInput("Movement", "Left") && InputSetting.KeyInput("Movement", "Right")) { Horizontal = 0; }
                if (!InputSetting.KeyInput("Movement", "Left") && !InputSetting.KeyInput("Movement", "Right")) { Horizontal = 0; }

                Vector3 dir = new Vector3(Horizontal, 0f, Vertical);
                transform.position += dir * MoveSpeed * Time.fixedDeltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), TurnSensitivity * Time.fixedDeltaTime);
            }

            velocity = GetComponent<Rigidbody>().velocity;
        }

        private void reset()
        {
            StopAllCoroutines();
            State = STATE.NONE;
            IsGrounded = true;
            CanJump = true;
            MoveSpeed = WalkSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                IsGrounded = true;
                InputSetting.InputUnLockAll();
                if (!CanJump) { StartCoroutine(CanRunToTrue()); }
            }
        }

        private IEnumerator CanRunToTrue()
        {
            yield return new WaitForSeconds(JUMP_DELAY);
            CanJump = true;
        }
    }
}
