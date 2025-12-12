#if UNITY_EDITOR
using ASUL.DNS.Save;
using ASUL.DNS.Window;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
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

            DialogueChoiceSaveData choiceData = new DialogueChoiceSaveData()
            {
                Text = "Output"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            var dataFoldout = DialogueElementUtility.CreateFoldout("Character");
            dataFoldout.AddToClassList("ds-node__custom-data-container");

            var nameField = DialogueElementUtility.CreateTextField(CharacterName, null, callback =>
            {
                Text = callback.newValue;
            });
            nameField.RegisterValueChangedCallback(evt => CharacterName = evt.newValue);
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

            Button addChoiceButton = DialogueElementUtility.CreateButton("Add choice", () =>
            {
                DialogueChoiceSaveData choiceData = new DialogueChoiceSaveData()
                {
                    Text = "New Choice"
                };

                Choices.Add(choiceData);

                Port choicePort = CreateChoicePort(choiceData);

                outputContainer.Add(choicePort);

            });

            addChoiceButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceButton);

            foreach (DialogueChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;

            DialogueChoiceSaveData choiceData = (DialogueChoiceSaveData)userData;

            Button deleteChoiseButton = DialogueElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1)
                {
                    return;
                }

                if (choicePort.connected)
                {
                    graphView.DeleteElements(choicePort.connections);
                }

                Choices.Remove(choiceData);
                graphView.RemoveElement(choicePort);
            });

            deleteChoiseButton.AddToClassList("ds-node__button");

            TextField choiceTextField = DialogueElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            choiceTextField.AddClasses("ds-node__choice-text-field", "ds-node__text-field__hidden");

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiseButton);
            return choicePort;
        }
    }
}
#endif