#if UNITY_EDITOR
using ASUL.DNS;
using ASUL.DNS.Save;
using ASUL.DNS.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public static class DialogueIOUtility
{

    private static DialogueGraphView graphView; 
    private static string graphFileName;
    private static string containerFolderPath;

    private static List<DialogueGroup> groups;
    private static List<DialogueNode> nodes;

    private static Dictionary<string, DialogueGroupSO> createdDialogueGroups;

    private static Dictionary<string, DialogueNodeSO> createdNodes;

    private static Dictionary<string, DialogueGroup> loadedGroups;

    private static Dictionary<string, DialogueNode> loadedNodes;

    public static string SavePath = $"Assets/Dialogues Data/";

    public static void Initialize(DialogueGraphView dialogueGraphView ,string graphName)
    {
        graphView = dialogueGraphView;
        graphFileName = graphName;
        containerFolderPath = SavePath + graphFileName;

        groups = new List<DialogueGroup>();
        nodes = new List<DialogueNode>();

        createdDialogueGroups = new Dictionary<string, DialogueGroupSO>();
        createdNodes = new Dictionary<string, DialogueNodeSO>();

        loadedGroups = new Dictionary<string, DialogueGroup>();
        loadedNodes = new Dictionary<string, DialogueNode>();
    }

    #region Save Methods

    public static void Save()
    {
        CreateStaticFolders();

        GetElementsFromGraphView();

        DialogueSaveDataSO dialogueData = CreateAsset<DialogueSaveDataSO>(SavePath, $"{graphFileName}");

        dialogueData.Initialize(graphFileName);

        DialogueSO dialogueContainer = CreateAsset<DialogueSO>(containerFolderPath, $"{graphFileName}");

        dialogueContainer.Initialize(graphFileName);

        SaveGroups(dialogueData, dialogueContainer);
        SaveNodes(dialogueData, dialogueContainer);

        SaveAsset(dialogueData);
        SaveAsset(dialogueContainer);


    }

    #endregion

    #region Nodes

    private static void SaveNodes(DialogueSaveDataSO dialogueData, DialogueSO dialogueContainer)
    {
        SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
        List<string> ungroupedNodeNames = new List<string>();
        foreach (DialogueNode node in nodes)
        {
            SaveNodeToDialogue(node, dialogueData);
            SaveNodeToScriptableObject(node, dialogueContainer);

            if (node.Group  != null )
            {
                groupedNodeNames.AddItem(node.Group.title, node.NodeName);
                continue;
            }

            ungroupedNodeNames.Add(node.NodeName);
        }
        UpdateNodeChoicesConnections();

        UpdateOldGroupedNodes(groupedNodeNames, dialogueData);
        UpdateOldUngroupedNodes(ungroupedNodeNames, dialogueData);
    }

    private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, DialogueSaveDataSO dialogueData)
    {
        if (dialogueData.OldGroupedNodeNames != null && dialogueData.OldGroupedNodeNames.Count != 0)
        {
            foreach (KeyValuePair<string, List<string>> oldGroupedNode in dialogueData.OldGroupedNodeNames)
            {
                List<string> nodesToRemove = new List<string>();

                if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                {
                    nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                }

                foreach (string nodeToRemove  in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                }

            }
        }

        dialogueData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
    }

    private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, DialogueSaveDataSO dialogueData)
    {
        if (dialogueData.OldUngroupedNodeNames != null && dialogueData.OldUngroupedNodeNames.Count != 0)
        {
            List<string> nodesToRemove = dialogueData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

            foreach (string nodeToRemove in nodesToRemove)
            {
                RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
            }
        }

        dialogueData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
    }

    private static void RemoveAsset(string path, string assetName)
    {
        AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
    }

    private static void UpdateNodeChoicesConnections()
    {
        foreach (DialogueNode node in nodes)
        {
            DialogueNodeSO dialogue = createdNodes[node.ID];

            for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex)
            {
                DialogueChoiceSaveData nodeChoice = node.Choices[choiceIndex];

                if (string.IsNullOrEmpty(nodeChoice.NodeID))
                {
                    continue;
                }

                dialogue.Choices[choiceIndex].NextDialogue = createdNodes[nodeChoice.NodeID];

                SaveAsset(dialogue);
            }
        }
    }

    private static void SaveNodeToDialogue(DialogueNode node, DialogueSaveDataSO dialogueData)
    {
        List<DialogueChoiceSaveData> choices = CloneNodeChoices(node.Choices);

        DialogueNodeSaveData nodeData = new DialogueNodeSaveData();

        if (node.DialogueNodeType == DialogueNodeTypes.PersonMultipleChoice || node.DialogueNodeType == DialogueNodeTypes.PersonSingleChoice)
        {
            nodeData.Character = node.Character;
        }

        nodeData.ID = node.ID;
        nodeData.Name = node.NodeName;
        nodeData.Text = node.Text;
        nodeData.Events = node.Events;
        nodeData.Choices = choices;
        nodeData.DialogueNodeType = node.DialogueNodeType;
        nodeData.GroupID = node.Group?.ID;
        nodeData.Position = node.GetPosition().position;
        nodeData.CharacterName = node.CharacterName;
        nodeData.CharacterNameColor = node.NameColor;
        nodeData.CharacterIcon = node.Icon;
        nodeData.DelayTime = node.DelayTime;
        nodeData.Voiceover = node.Voiceover;

        dialogueData.Nodes.Add(nodeData);
    }

    private static List<DialogueChoiceSaveData> CloneNodeChoices(List<DialogueChoiceSaveData> nodeChoices)
    {
        List<DialogueChoiceSaveData> choices = new List<DialogueChoiceSaveData>();

        foreach (DialogueChoiceSaveData choice in nodeChoices)
        {
            DialogueChoiceSaveData choiceSaveData = new DialogueChoiceSaveData()
            {
                Text = choice.Text,
                NodeID = choice.NodeID
            };

            choices.Add(choiceSaveData);
        }

        return choices;
    }

    private static void SaveNodeToScriptableObject(DialogueNode node, DialogueSO dialogueContainer)
    {
        DialogueNodeSO dialogueNode;

        if (node.Group != null)
        {
            dialogueNode = CreateAsset<DialogueNodeSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.NodeName);

            dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], dialogueNode);
        }
        else
        {
            dialogueNode = CreateAsset<DialogueNodeSO>($"{containerFolderPath}/Global/Dialogues", node.NodeName);

            dialogueContainer.UngroupedDialogues.Add(dialogueNode);
        }

        if (node.DialogueNodeType == DialogueNodeTypes.PersonMultipleChoice || node.DialogueNodeType == DialogueNodeTypes.PersonSingleChoice)
        {
            dialogueNode.Character = node.Character;
        }

        dialogueNode.Initialyze(
            node.NodeName,
            node.Text,
            node.Events,
            node.CharacterName,
            node.NameColor,
            node.Icon,
            node.Voiceover,
            node.DelayTime,
            ConvertNodeChoicesToDialogueChoices(node.Choices),
            node.DialogueNodeType,
            node.IsStartingNode()
        );

        createdNodes.Add(node.ID, dialogueNode);

        SaveAsset(dialogueNode);
    }

    private static List<DialogueChoiceData> ConvertNodeChoicesToDialogueChoices(List<DialogueChoiceSaveData> nodeChoises)
    {
        List<DialogueChoiceData> dialogueChoices = new List<DialogueChoiceData>();

        foreach (var choice in nodeChoises)
        {
            DialogueChoiceData choiceData = new DialogueChoiceData()
            {
                Text = choice.Text
            };

            dialogueChoices.Add(choiceData);
        }

        return dialogueChoices;
    }

    #endregion



    #region Groups
    private static void SaveGroups(DialogueSaveDataSO dialogueData, DialogueSO dialogueContainer)
    {
        List<string> groupNames = new List<string>();
        foreach (DialogueGroup group in groups)
        {
            SaveGroupToDialogue(group, dialogueData);
            SaveGroupToScriptableObject(group, dialogueContainer);

            groupNames.Add(group.title);
        }

        UpdateOldGroups(groupNames, dialogueData);
    }

    private static void UpdateOldGroups(List<string> currentGroupNames,DialogueSaveDataSO dialogueData)
    {
        if (dialogueData.OldGroupNames != null && dialogueData.OldGroupNames.Count != 0)
        {
            List<string> groupsToRemove = dialogueData.OldGroupNames.Except(currentGroupNames).ToList();

            foreach (string groupToRemove in groupsToRemove)
            {
                RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
            }
        }
        dialogueData.OldGroupNames = new List<string>(currentGroupNames);
    }

    private static void RemoveFolder(string path)
    {
        FileUtil.DeleteFileOrDirectory($"{path}.meta");
        FileUtil.DeleteFileOrDirectory($"{path}/");
    }

    private static void SaveGroupToScriptableObject(DialogueGroup group, DialogueSO dialogueContainer)
    {
        string groupName = group.title;

        CreateFolder($"{containerFolderPath}/Groups", groupName);
        CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

        DialogueGroupSO dialogueGroup = CreateAsset<DialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);

        dialogueGroup.Initialize(groupName);

        createdDialogueGroups.Add(group.ID, dialogueGroup);

        dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DialogueNodeSO>());

        SaveAsset(dialogueGroup);
    }

    private static void SaveGroupToDialogue(DialogueGroup group, DialogueSaveDataSO dialogueData)
    {
        DialogueGroupSaveData groupData = new DialogueGroupSaveData()
        {
            ID = group.ID,
            Name = group.title,
            Position = group.GetPosition().position
        };

        dialogueData.Groups.Add(groupData);

    }

    #endregion


    #region Fetch Methods

    private static void GetElementsFromGraphView()
    {
        Type groupType = typeof(DialogueGroup);

        graphView.graphElements.ForEach(graphElement =>
        {
            if (graphElement is DialogueNode node)
            {
                nodes.Add(node);

                return;
            }

            if(graphElement.GetType() == groupType)
            {
                DialogueGroup group = (DialogueGroup)graphElement;

                groups.Add(group);

                return;
            }

        });
    }

    #endregion


    public static void Load()
    {
        DialogueSaveDataSO dialogueData = LoadAsset<DialogueSaveDataSO>("Assets/Dialogues Data/", graphFileName);

        if (dialogueData == null)
        {
            EditorUtility.DisplayDialog(
                "Couldn't load te file!",
                "The file at the following path could not be found:\n\n" + 
                $"Assets/Dialogues Data/{graphFileName}\n\n" +
                "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                "OK"
            );
            return;
        }

        DialogueEditorWindow.UpdateFileName(dialogueData.FileName);

        LoadGroups(dialogueData.Groups);
        loadNodes(dialogueData.Nodes);
        LoadNodesConnections();
    }

    private static void LoadNodesConnections()
    {
        foreach (KeyValuePair<string, DialogueNode> loadedNode in loadedNodes)
        {
            foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
            {
                DialogueChoiceSaveData choiceData = (DialogueChoiceSaveData)choicePort.userData;

                if (string.IsNullOrEmpty(choiceData.NodeID))
                {
                    continue;
                }

                DialogueNode nextNode = loadedNodes[choiceData.NodeID];

                Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();

                Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                graphView.AddElement(edge);

                loadedNode.Value.RefreshPorts();
            }
        }
    }

    private static void loadNodes(List<DialogueNodeSaveData> nodes)
    {
        foreach (DialogueNodeSaveData nodeData in nodes)
        {
            List<DialogueChoiceSaveData> choices = CloneNodeChoices(nodeData.Choices);
            DialogueNode node = graphView.CreateNode(nodeData.Name, nodeData.DialogueNodeType, nodeData.Position, false);

            node.ID = nodeData.ID;
            node.Choices = choices;
            node.Text = nodeData.Text;
            node.Events = nodeData.Events;
            node.CharacterName = nodeData.CharacterName;
            node.NameColor = nodeData.CharacterNameColor;
            node.Icon = nodeData.CharacterIcon;
            node.DelayTime = nodeData.DelayTime;
            node.Voiceover = nodeData.Voiceover;

            if (nodeData.DialogueNodeType == DialogueNodeTypes.PersonMultipleChoice || nodeData.DialogueNodeType == DialogueNodeTypes.PersonSingleChoice)
            {
                node.Character = nodeData.Character;
            }


            node.Draw();

            graphView.AddElement(node);

            loadedNodes.Add(node.ID, node);

            if (string.IsNullOrEmpty(nodeData.GroupID))
            {
                continue;
            }

            DialogueGroup group = loadedGroups[nodeData.GroupID];

            node.Group = group;

            group.AddElement(node);
        }
    }

    private static void LoadGroups(List<DialogueGroupSaveData> groups)
    {
        foreach (DialogueGroupSaveData groupData in groups)
        {
            DialogueGroup group = graphView.CreateGroup(groupData.Name, groupData.Position);

            group.ID = groupData.ID;

            loadedGroups.Add(group.ID, group);
        }
    }

    #region Creation Methods

    private static void CreateStaticFolders()
    {
        CreateFolder("Assets", "Dialogues Data");
        CreateFolder("Assets/Dialogues Data", graphFileName);
        CreateFolder(containerFolderPath, "Global");
        CreateFolder(containerFolderPath, "Groups");
        CreateFolder($"{containerFolderPath}/Global", "Dialogues");
    }

    #endregion


    #region Utility Methods

    private static void CreateFolder(string path, string folderName)
    {
        if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
        {
            return;
        }

        AssetDatabase.CreateFolder(path, folderName);
    }

    private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        string fullPath = $"{path}/{assetName}.asset";
        T asset = LoadAsset<T>(path, assetName);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, fullPath);
        }

        return asset;
    }

    private static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        string fullPath = $"{path}/{assetName}.asset";

        return AssetDatabase.LoadAssetAtPath<T>(fullPath);
    }

    private static void SaveAsset(UnityEngine.Object asset)
    {
        EditorUtility.SetDirty(asset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    #endregion


}
#endif