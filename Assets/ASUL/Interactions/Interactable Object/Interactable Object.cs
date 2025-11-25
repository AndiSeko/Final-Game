using ASUL.EventSystem;
using UnityEngine;

namespace ASUL.Interactions
{
    [RequireComponent(typeof(Outline))]
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public virtual bool IsInteractable { get; set; } = true;

        public virtual bool IsInteract { get; set; } = false;

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
                outline.OutlineMode = outlineConfig.Mode;
            }
            catch
            {
                Debug.LogError($"Нет конфига обводки на объекте ({gameObject.name})!");
            }

            outline.enabled = false;
        }

        public virtual void Interact()
        {
            if (IsInteract)
                return;
            else
                IsInteract = true;
            EventManager.LaunchEvent(additionalEvents);
            IsInteract = false;
        }

        public virtual void SelectionToggle(bool isSelected)
        {
            outline.enabled = isSelected;
        }
    }
}
