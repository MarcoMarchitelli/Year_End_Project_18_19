using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUIHelper.ShowList(serializedObject.FindProperty("InputKeys"));

        serializedObject.ApplyModifiedProperties();
    }

}