using UnityEngine;
using UnityEditor;
using System;

public static class EditorGUIHelper
{
    private static readonly Texture2D backgroundTexture = Texture2D.whiteTexture;
    private static readonly GUIStyle textureStyle = new GUIStyle { normal = new GUIStyleState { background = backgroundTexture } };

    public static void DrawRect(Rect position, Color color, GUIContent content = null)
    {
        var backgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        GUI.Box(position, content ?? GUIContent.none, textureStyle);
        GUI.backgroundColor = backgroundColor;
    }

    public static void ShowList(SerializedProperty list, ListShowOptions options = ListShowOptions.Default)
    {
        bool
            showListLabel = (options & ListShowOptions.ListLabel) != 0,
            showListSize = (options & ListShowOptions.ListSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel++;
        }
        if (!showListLabel || list.isExpanded)
        {
            if (showListSize)
            {
                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            }
            ShowListElements(list);
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel--;
        }
    }

    private static void ShowListElements(SerializedProperty list)
    {
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
        }
    }

    [Flags]
    public enum ListShowOptions
    {
        None = 0,
        ListSize = 1,
        ListLabel = 2,
        Default = ListSize | ListLabel,
    }
}