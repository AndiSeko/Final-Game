using ASUL.Electronics;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : ElectronicRecipient
{
    private Animator animator;

    private void Awake()
    {
        try
        {
            animator = GetComponent<Animator>();
        }
        catch
        {
            Debug.LogError("Где аниматор?");
        }
    }

    public override void ToggleActive(bool active)
    {
        animator.SetBool("IsActive", active);
    }
}
