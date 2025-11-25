using ASUL.FPC;
using System;
using System.Collections;
using UnityEngine;

namespace ASUL.FPC.Animations
{
    public class FirstPersonAnimator : MonoBehaviour
    {
        public static Action<Animator> OnExternalKinemation;

        [SerializeField] private FirstPersonController controller;
        [SerializeField] private Animator animator;
        [SerializeField] private float damp = 10f;
        private float ySmoothed;

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform eyesZoneTransform;

        private bool isSafePeriod = true;
        private int frameCounter = 0;
        private const float MAX_SAFE_DISTANCE = 0.5f;

        public static bool IsIdleLocked = false;

        private IEnumerator Start()
        {
            yield return null;
        }

        private void Update()
        {
            if (animator == null || controller == null) return;

            animator.SetBool("IsMoving", controller.IsMoving && !IsIdleLocked);
            ySmoothed = Mathf.MoveTowards(ySmoothed, controller.Direction.y, damp * Time.deltaTime);
            animator.SetFloat("X", controller.Direction.x, 0.15f, Time.deltaTime);
            animator.SetFloat("Y", ySmoothed, 0.15f, Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (eyesZoneTransform == null || cameraTransform == null) return;

            if (isSafePeriod)
            {
                frameCounter++;

                float distance = Vector3.Distance(cameraTransform.position, eyesZoneTransform.position);

                if (distance > MAX_SAFE_DISTANCE)
                {
                    return;
                }

                if (frameCounter >= 3)
                {
                    isSafePeriod = false;
                }
            }

            cameraTransform.position = eyesZoneTransform.position;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (animator == null || cameraTransform == null) return;
            animator.SetLookAtWeight(1f, 0f, 1f, 0.3f, 0.5f);
            animator.SetLookAtPosition(cameraTransform.position + cameraTransform.forward * 10f);
            OnExternalKinemation?.Invoke(animator);
        }
    }
}