#if UNITY_EDITOR
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public static class DialogueElementUtility
{

    public static Foldout CreateFoldout(string text = null, bool collaplsed = false)
    {
        Foldout foldout = new Foldout() { text = text, value = !collaplsed };
        return foldout;
    }

    public static Button CreateButton(string text = null, Action onClick = null)
    {
        Button button = new Button(onClick) { text = text };
        return button;
    }

    public static Port CreatePort(this Node node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
    {
        var port = Port.Create<Edge>(orientation, direction, capacity, typeof(bool));
        port.portName = portName;
        return port;
    }

    public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
    {
        TextField textFileld = new TextField()
        {
            value = value,
            label = label,
        };

        if(onValueChanged != null)
        {
            textFileld.RegisterValueChangedCallback(onValueChanged);
        }
        return textFileld;
    }

    public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
    {
        TextField textArea = CreateTextField(value, label, onValueChanged);

        textArea.multiline = true;

        return textArea;
    }
}
#endif