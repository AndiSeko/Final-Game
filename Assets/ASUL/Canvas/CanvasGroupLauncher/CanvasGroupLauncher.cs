using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ASUL.Canvas
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupLauncher : MonoBehaviour
    {

        public bool IsOpened { get; protected set; }


        [SerializeField] protected bool isOpenedInAwake;

        protected CanvasGroup canvasGroup;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            ToggleOpen(isOpenedInAwake);
        }

        public void ToggleOpen(bool value)
        {
            if (value)
                Open();
            else
                Close();
        }

        protected virtual void Open(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            Open();
        }

        protected virtual void Close(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            Close();
        }


        protected virtual void Open()
        {
            IsOpened = true;
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        protected virtual void Close()
        {
            IsOpened = false;
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
