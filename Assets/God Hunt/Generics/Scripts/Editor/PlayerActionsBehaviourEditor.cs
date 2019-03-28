//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;

//[CustomEditor(typeof(PlayerActionsBehaviour))]
//public class PlayerActionsBehaviourEditor : Editor
//{
//    PlayerActionsBehaviour PlayerActions;
//    SerializedObject TargetObject;
//    SerializedProperty Actions;

//    List<bool> actionShowers;
//    Color defaultColor;

//    private void OnEnable()
//    {
//        PlayerActions = target as PlayerActionsBehaviour;
//        TargetObject = new SerializedObject(PlayerActions);
//        Actions = TargetObject.FindProperty("Actions");

//        actionShowers = new List<bool>();
//        for (int i = 0; i < Actions.arraySize; i++)
//        {
//            actionShowers.Add(false);
//        }

//        defaultColor = GUI.backgroundColor;
//    }

//    public override void OnInspectorGUI()
//    {
//        TargetObject.Update();
//        EditorStyles.foldout.fontStyle = FontStyle.Bold;

//        GUILayout.BeginVertical();
//        {
//            EditorGUILayout.Space();

//            for (int i = 0; i < Actions.arraySize; i++)
//            {
//                SerializedProperty actionRef = Actions.GetArrayElementAtIndex(i);

//                SerializedProperty Name = actionRef.FindPropertyRelative("Name");
//                SerializedProperty Input = actionRef.FindPropertyRelative("Input");
//                SerializedProperty InputKey = actionRef.FindPropertyRelative("InputKey");
//                SerializedProperty Hold = actionRef.FindPropertyRelative("Hold");
//                SerializedProperty HoldTime = actionRef.FindPropertyRelative("HoldTime");
//                SerializedProperty Duration = actionRef.FindPropertyRelative("Duration");
//                SerializedProperty InterruptOtherActions = actionRef.FindPropertyRelative("InterruptOtherActions");
//                SerializedProperty ActionsToInterrupt = actionRef.FindPropertyRelative("ActionsToInterrupt");

//                GUILayout.BeginVertical();
//                {
//                    GUILayout.BeginHorizontal();
//                    {
//                        actionShowers[i] = EditorGUILayout.Foldout(actionShowers[i], Name.stringValue, true, EditorStyles.foldout);
//                        EditorGUILayout.Space();
//                        GUI.backgroundColor = Color.red;
//                        if (GUILayout.Button("X", GUILayout.Width(20)))
//                        {
//                            PlayerActions.RemoveAction(i);
//                            actionShowers.RemoveAt(i);
//                        }
//                        GUI.backgroundColor = defaultColor;
//                    }
//                    GUILayout.EndHorizontal();

//                    if (actionShowers[i])
//                    {
//                        GUILayout.BeginVertical();
//                        {
//                            EditorGUILayout.Space();

//                            EditorGUILayout.PropertyField(Name);
//                            EditorGUILayout.Space();
//                            EditorGUILayout.PropertyField(Duration);
//                            EditorGUILayout.Space();
//                            Input.boolValue = EditorGUILayout.ToggleLeft("Has Input", Input.boolValue);
//                            if (Input.boolValue)
//                            {
//                                GUILayout.BeginVertical();
//                                {
//                                    EditorGUILayout.PropertyField(InputKey);
//                                    EditorGUILayout.PropertyField(Hold);
//                                    if (Hold.boolValue)
//                                        EditorGUILayout.PropertyField(HoldTime);
//                                }
//                                GUILayout.EndVertical();
//                            }

//                            EditorGUILayout.Space();

//                            InterruptOtherActions.boolValue = EditorGUILayout.ToggleLeft("Interrupts other actions", InterruptOtherActions.boolValue);
//                            if (InterruptOtherActions.boolValue)
//                            {
//                                if (PlayerActions.Actions[i].ActionsToInterrupt.Count > 0)
//                                {
//                                    for (int k = 0; k < PlayerActions.Actions[i].ActionsToInterrupt.Count; k++)
//                                    {
//                                        GUILayout.BeginHorizontal();
//                                        {
//                                            SerializedProperty actionToInterruptRef = Actions.GetArrayElementAtIndex(k);
//                                            SerializedProperty actionToInterruptName = actionToInterruptRef.FindPropertyRelative("Name");

//                                            EditorGUILayout.LabelField(actionToInterruptName.stringValue, EditorStyles.boldLabel);

//                                            GUI.backgroundColor = Color.red;
//                                            if (GUILayout.Button("X", GUILayout.Width(15)))
//                                            {
//                                                PlayerActions.Actions[i].RemoveActionToInterrupt(k);
//                                            }
//                                            GUI.backgroundColor = defaultColor;
//                                        }
//                                        GUILayout.EndHorizontal();
//                                    }
//                                }

//                                //button to select other actions to interrupt.
//                                if (GUILayout.Button("Select Action to interrupt"))
//                                {
//                                    GUILayout.BeginVertical();
//                                    for (int j = 0; j < Actions.arraySize; j++)
//                                    {
//                                        if (PlayerActions.Actions[i] == PlayerActions.Actions[j])
//                                            continue;

//                                        SerializedProperty actionToInterruptRef = Actions.GetArrayElementAtIndex(j);
//                                        SerializedProperty actionToInterruptName = actionToInterruptRef.FindPropertyRelative("Name");

//                                        if (GUILayout.Button(actionToInterruptName.stringValue))
//                                        {
//                                            PlayerActions.Actions[i].AddActionToInterrupt(PlayerActions.Actions[j]);
//                                        }
//                                    }
//                                    GUILayout.EndVertical();
//                                }
//                            }
//                        }
//                        GUILayout.EndVertical();

//                        EditorGUILayout.Space();
//                    }

//                    EditorGUILayout.Space();

//                }
//                GUILayout.EndVertical();
//            }

//            if (GUILayout.Button("Add New Skill"))
//            {
//                PlayerActions.AddAction();
//                actionShowers.Add(false);
//            }

//        }
//        GUILayout.EndVertical();

//        TargetObject.ApplyModifiedProperties();

//    }

//}