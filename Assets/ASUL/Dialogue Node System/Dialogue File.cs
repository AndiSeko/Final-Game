using ASUL.Localization;
using UnityEngine;

namespace ASUL.DNS.Save
{
    [CreateAssetMenu(fileName = "DialogueFile", menuName = "ASUL/DialogueFile")]
    public class DialogueFile : ScriptableObject
    {
        public SerializableDictionary<ASUL.Localization.Localization, DialogueSO> Localizations;
    }
}