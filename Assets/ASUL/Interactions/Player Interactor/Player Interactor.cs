using UnityEngine;

namespace ASUL.Interactions
{
    public class PlayerInteractor : MonoBehaviour
    {

        [SerializeField] private float interactionDistance = 1f;
        [SerializeField] private LayerMask interactableMask;

        [SerializeField] private PlayerInteractorViewier viewier;

        private InteractableObject currentTarget;


        private void Update()
        {
            Detection();
        }


        private void Detection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableMask))
            {
                var interactable = hit.collider.GetComponent<InteractableObject>();

                if (interactable != null)
                {
                    if(currentTarget != interactable)
                    {
                        if (currentTarget != null)
                            currentTarget.SelectionToggle(false);
                        
                        currentTarget = interactable;
                        currentTarget.SelectionToggle(true);
                        viewier.ToggleCursorSelection(true);
                    }
                }

            }
            else
            {
                if(currentTarget != null)
                {
                    viewier.ToggleCursorSelection(false);
                    currentTarget.SelectionToggle(false);
                    currentTarget = null;
                }
            }
        }
    }
}