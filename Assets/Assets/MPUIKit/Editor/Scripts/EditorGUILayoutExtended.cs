using System;
using UnityEditor;
using UnityEngine;

namespace MPUIKIT.Editor
{
    public class EditorGUILayoutExtended : UnityEditor.Editor
    {
        public static float FloatField(GUIContent _content, float _value, float _labelwidth,
            params GUILayoutOption[] _options)
        {
            Rect totalRect = EditorGUILayout.GetControlRect(_options);
            Rect labelRect = new Rect(totalRect.x, totalRect.y, _labelwidth, totalRect.height);
            Rect inputRect = new Rect(totalRect.x + _labelwidth, totalRect.y, totalRect.width - _labelwidth,
                totalRect.height);

            EditorGUI.LabelField(labelRect, _content);
            return EditorGUI.FloatField(inputRect, _value);
        }

        public static float FloatFieldExtended(Rect _position, float _value, Rect _dragHotZone)
        {
            return EditorGUI.FloatField(_position, _value); // Игнорируем _dragHotZone
        }
    }
}