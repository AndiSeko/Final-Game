using ASUL.FPC;
using UnityEngine;

public class FirstPersonAnimator : MonoBehaviour
{
    [SerializeField] private FirstPersonController controller;
    [SerializeField] private Animator animator;

    [SerializeField] private float damp = 10f;
    private float ySmoothed;

    private void Update()
    {
        animator.SetBool("IsMoving", controller.IsMoving);

        ySmoothed = Mathf.MoveTowards(ySmoothed, controller.Direction.y, damp * Time.deltaTime);

        animator.SetFloat("X", controller.Direction.x, 0.15f, Time.deltaTime);
        animator.SetFloat("Y", ySmoothed, 0.15f, Time.deltaTime);
    }
}
