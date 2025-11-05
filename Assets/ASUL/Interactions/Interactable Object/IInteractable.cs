
namespace ASUL.Interactions
{
    public interface IInteractable
    {
        public bool IsInteractable { get; set; }

        public void Interact();

        public void SelectionToggle(bool isSelected);
    }
}