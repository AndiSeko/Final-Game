using UnityEngine;
using UnityEngine.UI;

namespace ASUL.Interactions
{
    public class PlayerInteractorViewier : MonoBehaviour
    {
        [SerializeField] private Image cursor;
        [SerializeField] private Animator cursorAnimator;

        public void ToggleCursorSelection(bool value)
        {
            try
            {
                Color cursorColor = cursor.color;
                cursorColor.a = value ? 1f : 0.5f;
                cursor.color = cursorColor;

                cursorAnimator.SetBool("IsSelected", value);
            }
            catch
            {
                Debug.LogError("Не задан курсор!!!");
            }
        }
    }

}