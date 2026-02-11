using System;
using UnityEngine;
using UnityEngine.Localization;

namespace ASUL.DNS.Save
{
    [Serializable]
    public class DialogueChoiceSaveData
    {
        public LocalizedString Text { get; set; } = new LocalizedString("Dialogues", "");
        [field: SerializeField] public string NodeID { get; set; }
    }
}