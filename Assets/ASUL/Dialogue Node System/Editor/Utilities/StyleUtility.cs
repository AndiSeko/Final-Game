using UnityEditor;
using UnityEngine.UIElements;

public static class StyleUtility
{
    /////////////////////////////
    // StyleUtility (Experimental!)
    // © ZATUP TRIBUNAL 2025
    // Created by Andi Seko
    // Version 1.0
    /////////////////////////////

    public static VisualElement AddClasses(this VisualElement element, params string[] ClassNames)
    {
        foreach (var className in ClassNames)
        {
            element.AddToClassList(className);
        }

        return element;
    }

    public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetNames)
    {
        foreach (var styleSheetName in styleSheetNames)
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load(styleSheetName);
            element.styleSheets.Add(styleSheet);
        }
        return element;
    }



}
