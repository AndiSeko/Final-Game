using ASUL.DNS.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ASUL.DNS
{
    public class DialogueNodeSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField][field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public string Events { get; set; }
        [field: SerializeField] public string CharacterName { get; set; }
        [field: SerializeField] public Color CharacterNameColor { get; set; }
        [field: SerializeField] public Sprite CharacterIcon { get; set; }
        [field: SerializeField] public CharacterData Character { get; set; }
        [field: SerializeField] public AudioClip Voiceover { get; set; }
        [field: SerializeField] public float DelayTime { get; set; }
        [field: SerializeField] public List<DialogueChoiceData> Choices { get; set; }
        [field: SerializeField] public DialogueNodeTypes DialogueNodeType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialyze(string dialogueName, string text, string events, string characterName, Color characterNameColor, Sprite characterIcon, AudioClip voiceover, float delay, List<DialogueChoiceData> choices, DialogueNodeTypes dialogueNodeType, bool isStartingDialogue)
        {
            DialogueName = dialogueName;
            Text = text;
            Events = events;
            CharacterName = characterName;
            CharacterNameColor = characterNameColor;
            CharacterIcon = characterIcon;
            Choices = choices;
            DialogueNodeType = dialogueNodeType;
            IsStartingDialogue = isStartingDialogue;
            Voiceover = voiceover;
            DelayTime = delay;
        }
    }
}