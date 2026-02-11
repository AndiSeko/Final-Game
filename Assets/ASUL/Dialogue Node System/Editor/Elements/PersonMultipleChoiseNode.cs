#if UNITY_EDITOR
using ASUL.DNS.Data;
using ASUL.DNS.Save;
using ASUL.DNS.Window;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using System.Linq;

namespace ASUL.DNS
{
    public class PersonMultipleChoiceNode : DialogueNode
    {
        public override string CharacterName => Character != null ? Character.Name : "";
        public override Color NameColor => Character != null ? Character.NameColor : Color.white;
        public override Sprite Icon => Character != null ? Character.Icon : null;

        public override void Initialize(string nodeName, DialogueGraphView DSgraphView, Vector2 position)
        {
            base.Initialize(nodeName, DSgraphView, position);
            DialogueNodeType = DialogueNodeTypes.PersonMultipleChoice;

            DialogueChoiceSaveData choiceData = new DialogueChoiceSaveData
            {
                Text = new LocalizedString("Dialogues", "")
            };
            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            Foldout characterFoldout = DialogueElementUtility.CreateFoldout("Character");
            ObjectField characterObjectField = new ObjectField
            {
                objectType = typeof(CharacterData),
                allowSceneObjects = false,
                value = Character
            };
            characterObjectField.RegisterValueChangedCallback(evt => Character = evt.newValue as CharacterData);
            characterFoldout.Add(characterObjectField);
            extensionContainer.Add(characterFoldout);

            base.Draw();

            Button addChoiceButton = DialogueElementUtility.CreateButton("Add choice", AddNewChoice);
            addChoiceButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, addChoiceButton);

            RefreshChoicesVisual();
        }

        private void AddNewChoice()
        {
            DialogueChoiceSaveData choiceData = new DialogueChoiceSaveData
            {
                Text = new LocalizedString("Dialogues", "")
            };
            Choices.Add(choiceData);
            RefreshChoicesVisual();
            RefreshExpandedState();
        }

        private void RefreshChoicesVisual()
        {
            for (int i = outputContainer.childCount - 1; i >= 0; i--)
            {
                if (outputContainer[i] is Port port && port.direction == Direction.Output)
                    outputContainer.RemoveAt(i);
            }

            var tableNames = new List<string>();
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets("t:StringTableCollection");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var collection = AssetDatabase.LoadAssetAtPath<StringTableCollection>(path);
                if (collection != null) tableNames.Add(collection.TableCollectionName);
            }
#endif
            if (!tableNames.Any()) tableNames.Add("Dialogues");

            foreach (DialogueChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice, tableNames);
                outputContainer.Add(choicePort);
            }
        }

        private Port CreateChoicePort(DialogueChoiceSaveData choiceData, List<string> tableNames)
        {
            Port choicePort = this.CreatePort();
            choicePort.userData = choiceData;

            VisualElement container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;

            string currentTable = choiceData.Text.TableReference.TableCollectionName ?? "Dialogues";
            if (!tableNames.Contains(currentTable)) currentTable = tableNames.FirstOrDefault() ?? "Dialogues";

            PopupField<string> tableField = new PopupField<string>("Table", tableNames, currentTable);
            tableField.RegisterValueChangedCallback(evt => choiceData.Text.TableReference = evt.newValue);

            TextField keyField = new TextField("Key") { value = choiceData.Text.TableEntryReference.Key ?? "" };
            keyField.RegisterValueChangedCallback(evt => choiceData.Text.TableEntryReference = evt.newValue);

            Button deleteButton = DialogueElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count <= 1) return;
                if (choicePort.connected) graphView.DeleteElements(choicePort.connections);
                Choices.Remove(choiceData);
                graphView.RemoveElement(choicePort);
                RefreshChoicesVisual();
            });
            deleteButton.AddToClassList("ds-node__button");

            container.Add(tableField);
            container.Add(keyField);
            container.Add(deleteButton);
            choicePort.Add(container);

            return choicePort;
        }
    }
}
#endif