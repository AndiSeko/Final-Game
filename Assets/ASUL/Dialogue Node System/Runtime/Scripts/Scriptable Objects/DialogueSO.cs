using System.Collections.Generic;
using UnityEngine;

namespace ASUL.DNS
{
    public class DialogueSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public SerializableDictionary<DialogueGroupSO, List<DialogueNodeSO>> DialogueGroups { get; set; }
        [field: SerializeField] public List<DialogueNodeSO> UngroupedDialogues { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            DialogueGroups = new SerializableDictionary<DialogueGroupSO, List<DialogueNodeSO>>();
            UngroupedDialogues = new List<DialogueNodeSO>();
        }

    }
}