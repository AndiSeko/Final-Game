#if UNITY_EDITOR
using ASUL.DNS;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ASUL.DNS.Window
{
    public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueGraphView graphView;
        private Texture2D indentationIcon;

        public void Initialize(DialogueGraphView dialogueGraphView)
        {
            graphView = dialogueGraphView;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Create Element")),
            new SearchTreeGroupEntry(new GUIContent("Create Multiple Node"), 1),
            new SearchTreeEntry(new GUIContent("Create Person Multiple Choise Node", indentationIcon))
            {
                level = 2,
                userData = DialogueNodeTypes.PersonMultipleChoice
            },
            new SearchTreeEntry(new GUIContent("Create None Person Multiple Choise Node", indentationIcon))
            {
                level = 2,
                userData = DialogueNodeTypes.NonePersonMultipleChoice
            },
            new SearchTreeGroupEntry(new GUIContent("Create Single Node"), 1),
            new SearchTreeEntry(new GUIContent("Create Person Single Choise Node", indentationIcon))
            {
                level = 2,
                userData = DialogueNodeTypes.PersonSingleChoice
            },
            new SearchTreeEntry(new GUIContent("Create None Person Single Choise Node", indentationIcon))
            {
                level = 2,
                userData = DialogueNodeTypes.NonePersonSingleChoice
            },
            new SearchTreeGroupEntry(new GUIContent("Create Group"), 1),
            new SearchTreeEntry(new GUIContent("Create Group", indentationIcon))
            {
                level = 2,
                userData = new Group()
            }
        };

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (SearchTreeEntry.userData)
            {
                case DialogueNodeTypes.PersonMultipleChoice:
                    {
                        PersonMultipleChoiceNode ChoiceNode = (PersonMultipleChoiceNode)graphView.CreateNode("node", DialogueNodeTypes.PersonMultipleChoice, localMousePosition);

                        graphView.AddElement(ChoiceNode);

                        break;
                    }
                case DialogueNodeTypes.NonePersonMultipleChoice:
                    {
                        NonePersonMultipleChoiceNode ChoiceNode = (NonePersonMultipleChoiceNode)graphView.CreateNode("node", DialogueNodeTypes.NonePersonMultipleChoice, localMousePosition);

                        graphView.AddElement(ChoiceNode);

                        break;
                    }
                case DialogueNodeTypes.PersonSingleChoice:
                    {
                        PersonSingleChoiceNode ChoiceNode = (PersonSingleChoiceNode)graphView.CreateNode("node", DialogueNodeTypes.PersonSingleChoice, localMousePosition);

                        graphView.AddElement(ChoiceNode);
                        break;
                    }
                case DialogueNodeTypes.NonePersonSingleChoice:
                    {
                        NonePersonSingleChoiceNode ChoiceNode = (NonePersonSingleChoiceNode)graphView.CreateNode("node", DialogueNodeTypes.NonePersonSingleChoice, localMousePosition);

                        graphView.AddElement(ChoiceNode);
                        break;
                    }
                case Group _:
                    {
                        graphView.CreateGroup("Dialogue Group", localMousePosition);

                        break;
                    }
                default:
                    return false;
            }
            return true;
        }
    }
}
#endif