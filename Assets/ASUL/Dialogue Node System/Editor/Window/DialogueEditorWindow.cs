#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ASUL.DNS.Window
{
    public class DialogueEditorWindow : EditorWindow
    {

        private DialogueGraphView graphView;
        private readonly string defaultFileName = "DialogueFileName";
        private static TextField fileNameTextField;
        private Button saveButton;
        private Button miniMapButton;

        [MenuItem("Window/ASUL/Dialogue Node System")]
        public static void OpenDialogueEditor()
        {
            var window = GetWindow<DialogueEditorWindow>();
            window.titleContent = new GUIContent("Dialogue Editor");
        }


        private void OnEnable()
        {
            CreateDialog();
            AddToolbar();
            AddStyles();
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = DialogueElementUtility.CreateTextField(defaultFileName, "File Name: ", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            saveButton = DialogueElementUtility.CreateButton("Save", () =>
            {
                Save();
            });

            Button loadButton = DialogueElementUtility.CreateButton("Load", () => load());
            Button clearButton = DialogueElementUtility.CreateButton("Clear", () => Clear());
            Button resetButton = DialogueElementUtility.CreateButton("New Dialogue", () => ResetGraph());
            miniMapButton = DialogueElementUtility.CreateButton("Minimap", () => ToggleMiniMap());

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(resetButton);
            toolbar.Add(clearButton);
            toolbar.Add(miniMapButton);

            toolbar.AddStyleSheets("Assets/Editor Resourses/Dialogue System/DialogueToolbarStyles.uss");

            rootVisualElement.Add(toolbar);

        }

        private void ToggleMiniMap()
        {
            graphView.ToggleMiniMap();

            miniMapButton.ToggleInClassList("ds-toolbar__button__selected");
        }

        private void load()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogues", "Assets/Dialogues Data", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();

            DialogueIOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
            DialogueIOUtility.Load();

        }

        private void ResetGraph()
        {
            Clear();
            UpdateFileName(defaultFileName);
        }

        private void Clear()
        {
            graphView.ClearGraph();
        }

        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog(
                    "Invalid file name!",
                    "Please, write someone in FileName",
                    "OK"
                    );
                return;
            }
            DialogueIOUtility.Initialize(graphView, fileNameTextField.value);
            DialogueIOUtility.Save();
        }

        private void CreateDialog()
        {
            graphView = new DialogueGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Assets/Editor Resourses/Dialogue System/DialogueVariables.uss");
        }

        public void SetEnableSaving(bool isEnable)
        {
            saveButton.SetEnabled(isEnable);
        }
    }
}
#endif