using ASUL.DNS;
using UnityEngine;

public class DialogueReader : MonoBehaviour
{
    [SerializeField] DialogueWindowViewier viewier;

    public static DialogueReader Instance { get; private set; }

    private DialogueSO currentDialogue;

    private bool isExecute;

    private void Awake()
    {
        Instance = this;
    }

    public void LaunchDialogue(DialogueSO dialogue)
    {
        if (dialogue != currentDialogue)
            ClosePreviousDialogue();

        currentDialogue = dialogue;

        ExecuteDialogue();
    }

    private void ExecuteDialogue()
    {
        isExecute = true;
    }

    private void ClosePreviousDialogue()
    {
        isExecute = false;


    }
}
