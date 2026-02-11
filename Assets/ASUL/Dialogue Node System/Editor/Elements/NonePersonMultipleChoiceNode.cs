#if UNITY_EDITOR
using ASUL.DNS.Data;
using ASUL.DNS.Save;
using ASUL.DNS.Window;
using System.Collections.Generic;
using System.Linq; // Добавлено для LINQ (Any, FirstOrDefault и т.д.)
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace ASUL.DNS
{
    public class NonePersonMultipleChoiceNode : DialogueNode
    {
        public override void Initialize(string nodeName, DialogueGraphView DSgraphView, Vector2 position)
        {
            base.Initialize(nodeName, DSgraphView, position);
            DialogueNodeType = DialogueNodeTypes.NonePersonMultipleChoice;
            CharacterName = "noname";
            NameColor = Color.white;
            Icon = null;

            DialogueChoiceSaveData choiceData = new DialogueChoiceSaveData
            {
                Text = new LocalizedString("Dialogues", "")
            };
            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            var dataFoldout = DialogueElementUtility.CreateFoldout("Character");
            dataFoldout.AddToClassList("ds-node__custom-data-container");

            var nameField = DialogueElementUtility.CreateTextField(CharacterName, null, callback =>
            {
                CharacterName = callback.newValue;
            });
            nameField.AddToClassList("ds-node__textfield");
            dataFoldout.Add(nameField);

            var colorLabel = new Label("Name Color");
            var colorField = new ColorField { value = NameColor };
            colorField.RegisterValueChangedCallback(evt => NameColor = evt.newValue);
            dataFoldout.Add(colorLabel);
            dataFoldout.Add(colorField);

            var iconField = new ObjectField("Icon")
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = Icon
            };
            iconField.RegisterValueChangedCallback(evt => Icon = evt.newValue as Sprite);
            dataFoldout.Add(iconField);

            extensionContainer.Add(dataFoldout);

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
                if (collection != null)
                    tableNames.Add(collection.TableCollectionName);
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
            if (!tableNames.Contains(currentTable))
                currentTable = tableNames.FirstOrDefault() ?? "Dialogues";

            PopupField<string> tableField = new PopupField<string>("Table", tableNames, currentTable);
            tableField.RegisterValueChangedCallback(evt => choiceData.Text.TableReference = evt.newValue);

            TextField keyField = new TextField("Key")
            {
                value = choiceData.Text.TableEntryReference.Key ?? ""
            };
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