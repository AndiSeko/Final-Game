using System;
using UnityEngine;

namespace ASUL.DNS
{
    [Serializable]
    public class DialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DialogueNodeSO NextDialogue { get; set; }
    }
}