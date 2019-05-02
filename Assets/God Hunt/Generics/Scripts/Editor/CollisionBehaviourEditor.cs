using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CollisionBehaviour))]
public class CollisionBehaviourEditor : Editor
{
    CollisionBehaviour collisionBehaviour;
    SerializedObject TargetObject;
    SerializedProperty Collisions;

    List<bool> collisionShowers;
    Color defaultColor;

    private void OnEnable()
    {
        collisionBehaviour = target as CollisionBehaviour;
        TargetObject = new SerializedObject(collisionBehaviour);
        Collisions = TargetObject.FindProperty("collisions");

        collisionShowers = new List<bool>();
        for (int i = 0; i < Collisions.arraySize; i++)
        {
            collisionShowers.Add(false);
        }

        defaultColor = GUI.backgroundColor;
    }

    public override void OnInspectorGUI()
    {
        TargetObject.Update();
        EditorStyles.foldout.fontStyle = FontStyle.Bold;

        GUILayout.BeginVertical();
        {
            EditorGUILayout.Space();

            for (int i = 0; i < Collisions.arraySize; i++)
            {
                EditorGUI.indentLevel = 0;

                SerializedProperty actionRef = Collisions.GetArrayElementAtIndex(i);

                SerializedProperty collisionType = actionRef.FindPropertyRelative("collisionType");
                SerializedProperty eventType = actionRef.FindPropertyRelative("eventType");
                SerializedProperty searchFor = actionRef.FindPropertyRelative("searchFor");
                SerializedProperty layer = actionRef.FindPropertyRelative("layer");
                SerializedProperty component = actionRef.FindPropertyRelative("component");
                SerializedProperty tag = actionRef.FindPropertyRelative("tag");
                SerializedProperty onTagCollisionEnter = actionRef.FindPropertyRelative("onTagCollisionEnter");
                SerializedProperty onTagCollisionStay = actionRef.FindPropertyRelative("onTagCollisionStay");
                SerializedProperty onTagCollisionExit = actionRef.FindPropertyRelative("onTagCollisionExit");
                SerializedProperty onComponentCollisionEnter = actionRef.FindPropertyRelative("onComponentCollisionEnter");
                SerializedProperty onComponentCollisionStay = actionRef.FindPropertyRelative("onComponentCollisionStay");
                SerializedProperty onComponentCollisionExit = actionRef.FindPropertyRelative("onComponentCollisionExit");
                SerializedProperty onLayerCollisionEnter = actionRef.FindPropertyRelative("onLayerCollisionEnter");
                SerializedProperty onLayerCollisionStay = actionRef.FindPropertyRelative("onLayerCollisionStay");
                SerializedProperty onLayerCollisionExit = actionRef.FindPropertyRelative("onLayerCollisionExit");

                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        collisionShowers[i] = EditorGUILayout.Foldout(collisionShowers[i], "Collision " + i, true, EditorStyles.foldout);
                        EditorGUILayout.Space();
                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            collisionBehaviour.RemoveCollision(i);
                            collisionShowers.RemoveAt(i);
                        }
                        GUI.backgroundColor = defaultColor;
                    }
                    GUILayout.EndHorizontal();

                    EditorGUI.indentLevel++;

                    if (collisionShowers[i])
                    {
                        GUILayout.BeginVertical();
                        {
                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(collisionType);
                            EditorGUILayout.Space();
                            EditorGUILayout.PropertyField(eventType);
                            EditorGUILayout.Space();
                            EditorGUILayout.PropertyField(searchFor);
                            EditorGUILayout.Space();

                            switch (searchFor.enumValueIndex)
                            {
                                case 0:
                                    EditorGUILayout.PropertyField(component);
                                    EditorGUILayout.Space();
                                    switch (eventType.enumValueIndex)
                                    {
                                        case 0:
                                            EditorGUILayout.PropertyField(onComponentCollisionEnter);
                                            break;
                                        case 1:
                                            EditorGUILayout.PropertyField(onComponentCollisionStay);
                                            break;
                                        case 2:
                                            EditorGUILayout.PropertyField(onComponentCollisionExit);
                                            break;
                                    }
                                    break;
                                case 1:
                                    EditorGUILayout.PropertyField(layer);
                                    EditorGUILayout.Space();
                                    switch (eventType.enumValueIndex)
                                    {
                                        case 0:
                                            EditorGUILayout.PropertyField(onLayerCollisionEnter);
                                            break;
                                        case 1:
                                            EditorGUILayout.PropertyField(onLayerCollisionStay);
                                            break;
                                        case 2:
                                            EditorGUILayout.PropertyField(onLayerCollisionExit);
                                            break;
                                    }
                                    break;
                                case 2:
                                    EditorGUILayout.PropertyField(tag);
                                    EditorGUILayout.Space();
                                    switch (eventType.enumValueIndex)
                                    {
                                        case 0:
                                            EditorGUILayout.PropertyField(onTagCollisionEnter);
                                            break;
                                        case 1:
                                            EditorGUILayout.PropertyField(onTagCollisionStay);
                                            break;
                                        case 2:
                                            EditorGUILayout.PropertyField(onTagCollisionExit);
                                            break;
                                    }
                                    break;
                            }

                        }
                        GUILayout.EndVertical();

                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.Space();

                }
                GUILayout.EndVertical();
            }

            if (GUILayout.Button("Add New Collision"))
            {
                collisionBehaviour.AddCollision();
                collisionShowers.Add(false);
            }

        }
        GUILayout.EndVertical();

        TargetObject.ApplyModifiedProperties();

    }

}