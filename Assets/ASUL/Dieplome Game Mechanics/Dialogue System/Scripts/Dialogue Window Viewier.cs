using TMPro;
using UnityEngine;

public class DialogueWindowViewier : MonoBehaviour
{
    [SerializeField] private DialogueWindowLauncher launcher;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public void ToggleOpen(bool value)
    {
        launcher.ToggleOpen(value);
        dialogueText.text = "";
    }

    public void ChangeText(string text, Color color)
    {
        dialogueText.text = text;
        dialogueText.color = color;
    }
}
