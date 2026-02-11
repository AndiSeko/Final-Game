using ASUL.DNS;
using ASUL.DNS.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace ASUL.DNS.Save
{
    [Serializable]
    public class DialogueNodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public LocalizedString Text { get; set; }
        [field: SerializeField] public string Events { get; set; }
        [field: SerializeField] public string CharacterName { get; set; }
        [field: SerializeField] public Color CharacterNameColor { get; set; }
        [field: SerializeField] public Sprite CharacterIcon { get; set; }
        [field: SerializeField] public CharacterData Character { get; set; }
        [field: SerializeField] public List<DialogueChoiceSaveData> Choices { get; set; }
        [field: SerializeField] public AudioClip Voiceover { get; set; }
        [field: SerializeField] public float DelayTime { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public DialogueNodeTypes DialogueNodeType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}