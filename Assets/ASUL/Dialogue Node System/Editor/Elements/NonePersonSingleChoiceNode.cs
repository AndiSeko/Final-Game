#if UNITY_EDITOR
using ASUL.DNS.Save;
using ASUL.DNS.Window;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace ASUL.DNS
{
    public class NonePersonSingleChoiceNode : DialogueNode
    {

        public override void Initialize(string nodeName, DialogueGraphView DSgraphView, Vector2 position)
        {
            base.Initialize(nodeName, DSgraphView, position);

            DialogueNodeType = DialogueNodeTypes.NonePersonSingleChoice;
            CharacterName = "noname";
            NameColor = Color.white;
            Icon = null;

            DialogueChoiceSaveData choiceData = new DialogueChoiceSaveData()
            {
                Text = new LocalizedString("Dialogues", "")
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            var dataFoldout = new Foldout { text = "Character" };
            dataFoldout.AddToClassList("ds-node__custom-data-container");

            var nameField = new TextField("Name") { value = CharacterName };
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

            foreach (DialogueChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort("Output"); 
                choicePort.portName = "Output";
                choicePort.userData = choice;
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
#endif