using System;
using UnityEngine;

namespace ASUL.DNS.Save
{
    [Serializable]
    public class DialogueChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}