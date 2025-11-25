using UnityEngine;
using UnityEngine.UIElements;


namespace ASUL.Interactions.Animation
{
    public class DoubleChannelInteractableAnimatedObject : InteractableAnimatedObject
    {
        [SerializeField] private bool animationToggleValue = false;

        protected override void Animate()
        {
            IsInteractable = false;
            manipulator.IsActive = true;
            animationToggleValue = !animationToggleValue;
            animator.SetBool("Value", animationToggleValue);
            StartCoroutine(DelayAndEnd());
        }
    }
}

