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
    public class PersonSingleChoiceNode : DialogueNode
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

            DialogueNodeType = DialogueNodeTypes.PersonSingleChoice;

            DialogueChoiceSaveData choiceData = new DialogueChoiceSaveData()
            {
                Text = "Output"
            };

            Choices.Add(choiceData);

        }

        public override void Draw()
        {

            Foldout characterFoldout = new Foldout { text = "Character" };
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

            foreach (DialogueChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                choicePort.portName = choice.Text;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
#endif