using ASUL.FPC;
using UnityEngine;

public class FirstPersonAnimator : MonoBehaviour
{
    [SerializeField] private FirstPersonController controller;
    [SerializeField] private Animator animator;

    // Переход между анимациями blend tree
    [SerializeField] private float damp = 10f;
    private float ySmoothed;

    [SerializeField] private Transform headBone;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform eyesZoneTransform;

    private Quaternion baseLocalRotation;

    private void Start()
    {
        baseLocalRotation = headBone.localRotation;
    }

    private void Update()
    {
        animator.SetBool("IsMoving", controller.IsMoving);

        ySmoothed = Mathf.MoveTowards(ySmoothed, controller.Direction.y, damp * Time.deltaTime);

        animator.SetFloat("X", controller.Direction.x, 0.15f, Time.deltaTime);
        animator.SetFloat("Y", ySmoothed, 0.15f, Time.deltaTime);
    }


    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || cameraTransform == null) return;

        animator.SetLookAtWeight(1f, 0f, 1f, 0.3f, 0.5f);
        animator.SetLookAtPosition(cameraTransform.position + cameraTransform.forward * 10f);
    }

    private void LateUpdate()
    {
        cameraTransform.position = eyesZoneTransform.position;

    }

}
