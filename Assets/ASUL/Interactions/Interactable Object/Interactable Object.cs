using ASUL.EventSystem;
using UnityEngine;

namespace ASUL.Interactions
{
    [RequireComponent(typeof(Outline))]
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        public virtual bool IsInteractable { get; set; }

        public EventStep[] additionalEvents;

        public InteractableOutlineConfig outlineConfig;

        protected Outline outline;

        private protected virtual void Awake()
        {
            outline = GetComponent<Outline>();

            try
            {
                outline.OutlineWidth = outlineConfig.OutlineWidth;
                outline.OutlineColor = outlineConfig.OutlineColor;
            }
            catch
            {
                Debug.LogError($"Нет конфига обводки на объекте ({gameObject.name})!");
            }

            outline.enabled = false;
        }

        public virtual void Interact()
        {
            EventManager.LaunchEvent(additionalEvents);
            Animate();
        }

        public virtual void SelectionToggle(bool isSelected)
        {
            outline.enabled = isSelected;
        }

        protected virtual void Animate()
        {

        }
    }
}
