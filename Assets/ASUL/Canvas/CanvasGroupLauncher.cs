using UnityEngine;

namespace ASUL.Canvas
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupLauncher : MonoBehaviour
    {

        [SerializeField] protected bool isOpenedInAwake;

        protected CanvasGroup canvasGroup;

        protected void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            if (isOpenedInAwake)
            {
                Open();
            }
            else
            {
                Close();
            }
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
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        protected virtual void Close()
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
