using ASUL.FPC;
using System;
using UnityEngine;
namespace ASUL.FPC.Animations
{
    public class FirstPersonAnimator : MonoBehaviour
    {
        public static Action<Animator> OnExternalKinemation;
        
        [SerializeField] private FirstPersonController controller;
        
        [SerializeField] private Animator animator;
        
        // Переход между анимациями blend tree
        [SerializeField] private float damp = 10f;
        
        private float ySmoothed;
        
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform eyesZoneTransform;
        
        private void Update()
        {
            animator.SetBool("IsMoving", controller.IsMoving);
            ySmoothed = Mathf.MoveTowards(ySmoothed, controller.Direction.y, damp * Time.deltaTime);
            animator.SetFloat("X", controller.Direction.x, 0.15f, Time.deltaTime);
            animator.SetFloat("Y", ySmoothed, 0.15f, Time.deltaTime);

            if (eyesZoneTransform != null && cameraTransform != null)
            {
                Vector3 eyesPos = eyesZoneTransform.position;
                if (eyesPos.y < 0.5f) return;

                cameraTransform.position = eyesPos;
            }

        }

        void OnAnimatorIK(int layerIndex)
        {
            if (animator == null || cameraTransform == null) return;
            animator.SetLookAtWeight(1f, 0f, 1f, 0.3f, 0.5f);
            animator.SetLookAtPosition(cameraTransform.position + cameraTransform.forward * 10f);
            OnExternalKinemation?.Invoke(animator);
        }
    }
}