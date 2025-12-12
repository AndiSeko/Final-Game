#if UNITY_EDITOR
using ASUL.DNS.Data;
using ASUL.DNS.Save;
using ASUL.DNS.Window;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ASUL.DNS
{
    public class PersonMultipleChoiceNode : DialogueNode
    {

        public override string CharacterName
        {
            get => Character != null ? Character.Name : "";
        }
        public override Color NameColor
        {
            get => Character != null ? Character.NameColor : Color.white;
        }
        public override Sprite Icon
        {
            get => Character != null ? Character.Icon : null;
        }

        public override void Initialize(string nodeName, DialogueGraphView DSgraphView, Vector2 position)
        {
            base.Initialize(nodeName, DSgraphView, position);

            DialogueNodeType = DialogueNodeTypes.PersonMultipleChoice;

            DialogueChoiceSaveData choiceData = new DialogueChoiceSaveData()
            {
                Text = "Output"
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
            characterObjectField.RegisterValueChangedCallback(evt =>
            {
                Character = evt.newValue as CharacterData;
            });
            characterFoldout.Add(characterObjectField);

            extensionContainer.Add(characterFoldout);

            base.Draw();

            Button addChoiceButton = DialogueElementUtility.CreateButton("Add choice", () =>
            {

                DialogueChoiceSaveData choiceData = new DialogueChoiceSaveData()
                {
                    Text = "New Choice"
                };

                Port choicePort = CreateChoicePort(choiceData);

                Choices.Add(choiceData);

                outputContainer.Add(choicePort);

                RefreshPorts();
                RefreshExpandedState();
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