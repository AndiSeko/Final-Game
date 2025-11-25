using ASUL.FPC.Animations;
using System.Collections;
using UnityEngine;

namespace ASUL.Interactions.Animation
{
    [RequireComponent(typeof(BodyFixator))]
    public class InteractableAnimatedObject : InteractableObject
    {
        public BodyManipulator manipulator;

        public Animator animator;

        public BodyFixator fixator;

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

            fixator.Detaching();
        }

        protected virtual void Animate()
        {
            StartCoroutine(AnimateRoutine());
        }

        private IEnumerator AnimateRoutine()
        {
            fixator.Fixing();

            while (fixator.IsWorking)
                yield return null;

            IsInteractable = false;
            manipulator.IsActive = true;

            if (animator != null)
                animator.SetTrigger("Interact");
            else
                Debug.LogError("Animator не назначен!");

            StartCoroutine(DelayAndEnd());
        }
    }
}