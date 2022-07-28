using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using Mirror;

namespace XVerse.Player.Control
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Header("Camera Basic Setting")]
        [SerializeField] private Transform followTarget = null;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private KeyCode cameraMovementToggleKey = KeyCode.Escape;

        [Header("Camera Rotation")]
        [SerializeField] private float rotationSpeed = 500;
        [SerializeField] private Vector2 pitchAngleLimit = new Vector2(-30, 60f);

        [Header("Camera Zoom")]
        [SerializeField] private float zoomSensitivity = 4f;
        [SerializeField] private float zoomTime = 0.5f;
        [SerializeField] private Ease zoomEaseType = Ease.OutQuad;
        [SerializeField] private float initialCameraDistance = 8f;
        [SerializeField] private Vector2 cameraDistanceLimit = new Vector2(5, 13f);

        private bool cameraMovementEnabled = true;
        private Cinemachine3rdPersonFollow tpFollow;
        private Vector2 rotation = Vector2.zero;
        private float distance;

        /// <summary>
        /// Enables player camera controller and virtualcamera only when this is client authority.
        /// </summary>
        public override void OnStartAuthority()
        {
            enabled = true;
            virtualCamera.gameObject.SetActive(true);

            if (followTarget == null)
            {
                followTarget = transform.Find("Follow Target");

                if (followTarget == null)
                {
                    followTarget = transform;
                }
            }

            tpFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            distance = initialCameraDistance;
            virtualCamera.gameObject.SetActive(true);
            virtualCamera.m_Follow = followTarget;
            virtualCamera.m_LookAt = followTarget;
        }

        void Update()
        {
            ToggleCameraMovement();

            if (cameraMovementEnabled)
            {
                RotateCamera();
                ZoomCamera();
            }
        }

        /// <summary>
        /// Rotates player camera using mouse delta position.
        /// </summary>
        private void RotateCamera()
        {
            rotation.y += UnityEngine.Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
            rotation.y = rotation.y % 360;
            rotation.x += -UnityEngine.Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;
            rotation.x = Mathf.Clamp(rotation.x, pitchAngleLimit.x, pitchAngleLimit.y);
            followTarget.eulerAngles = (Vector2)rotation;
        }

        /// <summary>
        /// Zoom player camera using mouse scrollwheel value.
        /// </summary>
        private void ZoomCamera()
        {
            distance -= UnityEngine.Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
            distance = Mathf.Clamp(distance, cameraDistanceLimit.x, cameraDistanceLimit.y);
            DOTween.To(() => tpFollow.CameraDistance, x => tpFollow.CameraDistance = x, distance, zoomTime).SetEase(zoomEaseType);
        }

        /// <summary>
        /// Toggle camera movement using toggle key.
        /// </summary>
        private void ToggleCameraMovement()
        {
            if (UnityEngine.Input.GetKeyDown(cameraMovementToggleKey))
            {
                cameraMovementEnabled = !cameraMovementEnabled;
            }
        }
    }
}
