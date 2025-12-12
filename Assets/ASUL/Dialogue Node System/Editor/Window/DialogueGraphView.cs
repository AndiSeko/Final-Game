#if UNITY_EDITOR
using ASUL.DNS;
using ASUL.DNS.Error;
using ASUL.DNS.Save;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ASUL.DNS.Window
{
    public class DialogueGraphView : GraphView
    {

        private DialogueEditorWindow editorWindow;
        DSSearchWindow searchWindow;
        private MiniMap miniMap;

        private SerializableDictionary<string, DialogueNodeErrorData> ungroupedNodes;
        private SerializableDictionary<string, DialogueGroupErrorData> groups;
        private SerializableDictionary<Group, SerializableDictionary<string, DialogueNodeErrorData>> groupedNodes;

        private int repeatedNamesAmount;

        public int RepeatedNamesAmount
        {
            get
            {
                return repeatedNamesAmount;
            }
            set
            {
                repeatedNamesAmount = value;

                if (repeatedNamesAmount == 0)
                {
                    editorWindow.SetEnableSaving(true);
                }

                if (repeatedNamesAmount == 1)
                {
                    editorWindow.SetEnableSaving(false);
                }
            }
        }

        public DialogueGraphView(DialogueEditorWindow editor)
        {
            editorWindow = editor;

            ungroupedNodes = new SerializableDictionary<string, DialogueNodeErrorData>();
            groups = new SerializableDictionary<string, DialogueGroupErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DialogueNodeErrorData>>();

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();
            AddMiniMap();
            OnGroupElementAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnElementsDeleted();
            OnGraphViewChanged();
            AddStyles();
            AddMiniMapStyles();
        }

        private void AddMiniMapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(29, 29, 30, 225));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 225));

            miniMap.style.backgroundColor = backgroundColor;
            miniMap.style.borderBottomColor = borderColor;
            miniMap.style.borderTopColor = borderColor;
            miniMap.style.borderLeftColor = borderColor;
            miniMap.style.borderRightColor = borderColor;
        }

        private void AddMiniMap()
        {
            miniMap = new MiniMap()
            {
                anchored = true
            };

            miniMap.SetPosition(new Rect(15, 50, 200, 180));

            Add(miniMap);

            miniMap.visible = false;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                {
                    return;
                }
                if (startPort.node == port.node)
                {
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);

            });

            return compatiblePorts;
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
        private void AddStyles()
        {
            this.AddStyleSheets(
                    "Assets/Editor Resourses/Dialogue System/DialogueGraphViewStyles.uss",
                    "Assets/Editor Resourses/Dialogue System/DialogueNodeStyles.uss"
                );
        }
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(CreateNodeContextualMenu("Add Node (Person Multiple Choise)", DialogueNodeTypes.PersonMultipleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (None Person Multiple Choise)", DialogueNodeTypes.NonePersonMultipleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Person Single Choise)", DialogueNodeTypes.PersonSingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (None Person Single Choise)", DialogueNodeTypes.NonePersonSingleChoice));

            this.AddManipulator(CreateGroupContextualMenu());

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))));

            return contextualMenuManipulator;
        }

        public DialogueGroup CreateGroup(string title, Vector2 localMousePosition)
        {
            DialogueGroup group = new DialogueGroup(title, localMousePosition);
            AddGroup(group);
            AddElement(group);

            foreach (GraphElement selectedElement in selection)
            {
                if (!(selectedElement is DialogueNode))
                {
                    continue;
                }
                DialogueNode node = (DialogueNode)selectedElement;
                group.AddElement(node);
            }

            return group;
        }

        private void AddGroup(DialogueGroup group)
        {
            string groupName = group.title.ToLower();

            if (!groups.ContainsKey(groupName))
            {
                DialogueGroupErrorData groupErrorData = new DialogueGroupErrorData();

                groupErrorData.Groups.Add(group);

                groups.Add(groupName, groupErrorData);

                return;
            }

            List<DialogueGroup> groupsList = groups[groupName].Groups;

            groupsList.Add(group);

            Color errorColor = groups[groupName].ErrorData.Color;

            group.SetErrorStyle(errorColor);

            if (groupsList.Count == 2)
            {
                ++RepeatedNamesAmount;

                groupsList[0].SetErrorStyle(errorColor);
            }
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueNodeTypes dialogueNodeType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("node", dialogueNodeType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));

            return contextualMenuManipulator;
        }

        private static readonly Dictionary<DialogueNodeTypes, Type> nodeTypeMap = new Dictionary<DialogueNodeTypes, Type>
        {
            { DialogueNodeTypes.PersonMultipleChoice, typeof(PersonMultipleChoiceNode) },
            { DialogueNodeTypes.NonePersonMultipleChoice, typeof(NonePersonMultipleChoiceNode) },
            { DialogueNodeTypes.PersonSingleChoice, typeof(PersonSingleChoiceNode) },
            { DialogueNodeTypes.NonePersonSingleChoice, typeof(NonePersonSingleChoiceNode) }
        };

        public DialogueNode CreateNode(string nodeName, DialogueNodeTypes dialogueNodeType, Vector2 position, bool shouldDraw = true)
        {
            if (!nodeTypeMap.TryGetValue(dialogueNodeType, out var nodeType))
            {
                Debug.LogError($"DialogueNodeType {dialogueNodeType} not mapped to a class!");
                return null;
            }

            DialogueNode dialogueNode = (DialogueNode)Activator.CreateInstance(nodeType);
            dialogueNode.Initialize(nodeName, this, position);

            if (shouldDraw) dialogueNode.Draw();

            AddUngroupedNode(dialogueNode);
            AddElement(dialogueNode);
            return dialogueNode;
        }


        public void AddUngroupedNode(DialogueNode node)
        {
            string nodeName = node.NodeName.ToLower();

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                DialogueNodeErrorData nodeErrorData = new DialogueNodeErrorData();

                nodeErrorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, nodeErrorData);

                return;
            }

            List<DialogueNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Add(node);
            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ++RepeatedNamesAmount;
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(DialogueNode node)
        {
            string nodeName = node.NodeName.ToLower();

            List<DialogueNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Remove(node);

            node.ResetStyle();

            if (ungroupedNodesList.Count == 1)
            {
                --RepeatedNamesAmount;
                ungroupedNodesList[0].ResetStyle();

                return;
            }

            if (ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }
        }

        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();

                searchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, AskUser) =>
            {
                Type groupType = typeof(DialogueGroup);
                Type edgeType = typeof(Edge);

                List<DialogueGroup> groupsToDelete = new List<DialogueGroup>();
                List<Edge> edgesToDelete = new List<Edge>();
                List<DialogueNode> nodesToDelete = new List<DialogueNode>();

                foreach (GraphElement element in selection)
                {
                    if (element is DialogueNode node)
                    {
                        nodesToDelete.Add(node);

                        continue;
                    }

                    if (element.GetType() == edgeType)
                    {
                        Edge edge = (Edge)element;
                        edgesToDelete.Add(edge);
                        continue;
                    }


                    if (element.GetType() != groupType)
                    {
                        continue;
                    }
                    DialogueGroup group = (DialogueGroup)element;

                    groupsToDelete.Add(group);
                }

                foreach (DialogueGroup group in groupsToDelete)
                {
                    List<DialogueNode> groupNodes = new List<DialogueNode>();

                    foreach (GraphElement element in group.containedElements)
                    {
                        if (!(element is DialogueNode node))
                        {
                            continue;
                        }
                        DialogueNode groupNode = (DialogueNode)element;

                        groupNodes.Add(groupNode);
                    }

                    group.RemoveElements(groupNodes);

                    RemoveGroup(group);

                    RemoveElement(group);
                }

                DeleteElements(edgesToDelete);

                foreach (DialogueNode node in nodesToDelete)
                {
                    if (node.Group != null)
                    {
                        node.Group.RemoveElement(node);
                    }
                    RemoveUngroupedNode(node);

                    node.DisconnectAllPorts();

                    RemoveElement(node);
                }
            };
        }

        private void RemoveGroup(DialogueGroup group)
        {
            string oldGroupName = group.title.ToLower();

            if (!groups.ContainsKey(oldGroupName))
            {
                return;
            }

            List<DialogueGroup> groupsList = groups[oldGroupName].Groups;
            groupsList.Remove(group);
            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                --RepeatedNamesAmount;

                groupsList[0].ResetStyle();
                return;
            }

            if (groupsList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }
        }

        private void OnGroupElementAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DialogueNode))
                    {
                        continue;
                    }

                    DialogueGroup nodeGroup = (DialogueGroup)group;

                    DialogueNode node = (DialogueNode)element;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, nodeGroup);
                }
            };
        }

        public void AddGroupedNode(DialogueNode node, DialogueGroup group)
        {
            string nodeName = node.NodeName.ToLower();

            node.Group = group;

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, DialogueNodeErrorData>());
            }

            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                DialogueNodeErrorData nodeErrorData = new DialogueNodeErrorData();

                nodeErrorData.Nodes.Add(node);

                groupedNodes[group].Add(nodeName, nodeErrorData);

                return;
            }

            List<DialogueNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Add(node);

            Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++RepeatedNamesAmount;

                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DialogueNode))
                    {
                        continue;
                    }

                    DialogueNode node = (DialogueNode)element;

                    RemoveGroupedNode(node, group);
                    AddUngroupedNode(node);
                }
            };
        }

        public void RemoveGroupedNode(DialogueNode node, Group group)
        {
            string nodeName = node.NodeName.ToLower();

            node.Group = null;

            List<DialogueNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Remove(node);

            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                --RepeatedNamesAmount;
                groupedNodesList[0].ResetStyle();

                return;
            }

            if (groupedNodesList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);

                if (groupedNodes[group].Count == 0)
                {
                    groupedNodes.Remove(group);
                }
            }
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        DialogueNode nextNode = (DialogueNode)edge.input.node;

                        DialogueChoiceSaveData choiceData = (DialogueChoiceSaveData)edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);

                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = (Edge)element;

                        DialogueChoiceSaveData choiceData = (DialogueChoiceSaveData)edge.output.userData;

                        choiceData.NodeID = "";
                    }
                }

                return changes;
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DialogueGroup dialogueGroup = (DialogueGroup)group;
                string oldTitle = dialogueGroup.OldTitle;

                if (groups.ContainsKey(oldTitle))
                {
                    List<DialogueGroup> oldGroupsList = groups[oldTitle].Groups;
                    oldGroupsList.Remove(dialogueGroup);

                    if (oldGroupsList.Count == 1)
                    {
                        --RepeatedNamesAmount;
                        oldGroupsList[0].ResetStyle();
                    }
                    else if (oldGroupsList.Count == 0)
                    {
                        groups.Remove(oldTitle);
                    }
                }

                dialogueGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
                dialogueGroup.OldTitle = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(dialogueGroup.title))
                {
                    if (!string.IsNullOrEmpty(dialogueGroup.OldTitle))
                    {
                        ++RepeatedNamesAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(dialogueGroup.OldTitle))
                    {
                        --RepeatedNamesAmount;
                    }
                }

                if (groups.ContainsKey(newTitle))
                {
                    List<DialogueGroup> newGroupsList = groups[newTitle].Groups;
                    newGroupsList.Add(dialogueGroup);
                    Color errorColor = groups[newTitle].ErrorData.Color;
                    dialogueGroup.SetErrorStyle(errorColor);

                    if (newGroupsList.Count >= 2)
                    {
                        foreach (var grp in newGroupsList)
                        {
                            grp.SetErrorStyle(errorColor);
                        }
                    }
                }
                else
                {
                    DialogueGroupErrorData groupErrorData = new DialogueGroupErrorData();
                    groupErrorData.Groups.Add(dialogueGroup);
                    groups.Add(newTitle, groupErrorData);
                    dialogueGroup.ResetStyle();
                }
            };
        }
        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
            groups.Clear();
            groupedNodes.Clear();
            ungroupedNodes.Clear();

            RepeatedNamesAmount = 0;
        }

        public void ToggleMiniMap()
        {
            miniMap.visible = !miniMap.visible;
        }
    }
}
#endif