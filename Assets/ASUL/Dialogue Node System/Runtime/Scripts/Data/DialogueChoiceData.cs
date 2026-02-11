using System;
using UnityEngine;
using UnityEngine.Localization;

namespace ASUL.DNS
{
    [Serializable]
    public class DialogueChoiceData
    {
        [field: SerializeField] public LocalizedString Text { get; set; } = new LocalizedString("Dialogues", "");
        [field: SerializeField] public DialogueNodeSO NextDialogue { get; set; }
    }
}