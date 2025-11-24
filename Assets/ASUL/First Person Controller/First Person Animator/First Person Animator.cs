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

        private bool canSyncCamera = false;

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            canSyncCamera = true;
        }

        private void Update()
        {
            if (animator == null || controller == null) return;

            animator.SetBool("IsMoving", controller.IsMoving);
            ySmoothed = Mathf.MoveTowards(ySmoothed, controller.Direction.y, damp * Time.deltaTime);
            animator.SetFloat("X", controller.Direction.x, 0.15f, Time.deltaTime);
            animator.SetFloat("Y", ySmoothed, 0.15f, Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (!canSyncCamera) return;
            if (eyesZoneTransform == null || cameraTransform == null) return;

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