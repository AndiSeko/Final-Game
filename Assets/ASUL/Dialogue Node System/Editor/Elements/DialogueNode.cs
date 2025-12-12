#if UNITY_EDITOR
using ASUL.DNS.Data;
using ASUL.DNS.Save;
using ASUL.DNS.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ASUL.DNS
{
    public class DialogueNode : Node
    {
        public string ID { get; set; }

        public string NodeName { get; set; }

        public DialogueNodeTypes DialogueNodeType { get; set; }
        public DialogueGroup Group { get; set; }

        public string Text { get; set; }

        public List<DialogueChoiceSaveData> Choices { get; set; }

        public string Events { get; set; }

        public virtual string CharacterName { get; set; }
        public virtual Color NameColor { get; set; }
        public virtual Sprite Icon { get; set; }
        public virtual CharacterData Character { get; set; }

        public AudioClip Voiceover { get; set; }
        public float DelayTime { get; set; }


        protected DialogueGraphView graphView;
        private Color defaultBackgroundColor;

        public virtual void Initialize(string nodeName, DialogueGraphView DSgraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            NodeName = nodeName;
            Choices = new List<DialogueChoiceSaveData>();
            Text = "Type some text here...";
            Events = "";
            DelayTime = 0f;

            graphView = DSgraphView;
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectPorts(inputContainer));
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectPorts(outputContainer));

            base.BuildContextualMenu(evt);
        }

        public virtual void Draw()
        {
            TextField NodeNameTextField = DialogueElementUtility.CreateTextField(NodeName, null, callback =>
            {
                TextField target = (TextField)callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(NodeName))
                    {
                        ++graphView.RepeatedNamesAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(NodeName))
                    {
                        --graphView.RepeatedNamesAmount;
                    }
                }

                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);
                    NodeName = target.value;
                    graphView.AddUngroupedNode(this);
                    return;
                }

                DialogueGroup currentGroup = Group;

                graphView.RemoveGroupedNode(this, Group);

                NodeName = target.value;

                graphView.AddGroupedNode(this, currentGroup);
            });

            NodeNameTextField.AddClasses("unity-text-field__input", "ds-node__filename-text-field", "ds-node__text-field__hidden");

            titleContainer.Insert(0, NodeNameTextField);

            Port inputPort = this.CreatePort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = DialogueElementUtility.CreateFoldout("Text");
            TextField textTextField = DialogueElementUtility.CreateTextArea(Text, null, callback =>
            {
                Text = callback.newValue;
            });
            textTextField.AddClasses("unity-text-field__input", "ds-node__quote-text-field");
            textFoldout.Add(textTextField);

            Foldout eventsFoldout = DialogueElementUtility.CreateFoldout("Events");
            TextField eventsTextField = DialogueElementUtility.CreateTextField(Events, null, callback =>
            {
                Events = callback.newValue;
            });
            eventsTextField.AddClasses("unity-text-field__input", "ds-node__quote-text-field");
            eventsFoldout.Add(eventsTextField);

            customDataContainer.Add(textFoldout);
            customDataContainer.Add(eventsFoldout);

            extensionContainer.Add(customDataContainer);

            Foldout voiceoverFoldout = DialogueElementUtility.CreateFoldout("Voiceover");
            ObjectField voiceoverObjectField = new ObjectField
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = Voiceover
            };
            voiceoverObjectField.RegisterValueChangedCallback(evt =>
            {
                Voiceover = evt.newValue as AudioClip;
            });
            voiceoverFoldout.Add(voiceoverObjectField);

            extensionContainer.Add(voiceoverFoldout);

            Foldout delayFoldout = DialogueElementUtility.CreateFoldout("Delay");

            FloatField delayField = new FloatField("Delay")
            {
                value = DelayTime
            };
            delayField.RegisterValueChangedCallback(evt =>
            {
                DelayTime = evt.newValue;
            });

            delayFoldout.Add(delayField);
            extensionContainer.Add(delayFoldout);
        }

        public void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port)inputContainer.Children().First();

            return !inputPort.connected;
        }
    }
}
#endif