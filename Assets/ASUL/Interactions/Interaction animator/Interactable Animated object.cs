using ASUL.FPC.Animations;
using System.Collections;
using UnityEngine;

namespace ASUL.Interactions.Animation
{
    public class InteractableAnimatedObject : InteractableObject
    {
        public BodyManipulator manipulator;

        public Animator animator;

        public float delaytime = 1.0f;

        public override void Interact()
        {
            base.Interact();
            Animate();
        }

        public IEnumerator DelayAndEnd()
        {
            yield return new WaitForSeconds(delaytime);
            manipulator.IsActive = false;
            IsInteractable = true;
        }

        protected virtual void Animate()
        {
            IsInteractable = false;
            manipulator.IsActive = true;
            animator.SetTrigger("Interact");
            StartCoroutine(DelayAndEnd());
        }
    }
}