using UnityEditor;

[CustomEditor(typeof(PatrolBehaviour))]
[CanEditMultipleObjects]
public class PatrolBehaviourEditor : Editor
{
    SerializedProperty path, speed, waitTime, rotationAnglePerSecond, rotatesToWaypoint, OnMovementStart, OnMovementEnd, OnWaypointReached, OnPathFinished, patrolOnStart;

    bool showReferences = true, showBehaviours = true, showParameters = true, showEvents = true;

    private void OnEnable()
    {
        path = serializedObject.FindProperty("path");
        speed = serializedObject.FindProperty("speed");
        waitTime = serializedObject.FindProperty("waitTime");
        rotationAnglePerSecond = serializedObject.FindProperty("rotationAnglePerSecond");
        rotatesToWaypoint = serializedObject.FindProperty("rotatesToWaypoint");
        OnMovementStart = serializedObject.FindProperty("OnMovementStart");
        OnMovementEnd = serializedObject.FindProperty("OnMovementEnd");
        OnWaypointReached = serializedObject.FindProperty("OnWaypointReached");
        OnPathFinished = serializedObject.FindProperty("OnPathFinished");
        patrolOnStart = serializedObject.FindProperty("patrolOnStart");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorStyles.foldout.fontStyle = UnityEngine.FontStyle.Bold;

        EditorGUILayout.Space();

        showReferences = EditorGUILayout.Foldout(showReferences, "References", true, EditorStyles.foldout);
        if (showReferences)
        {
            EditorGUILayout.PropertyField(path);
        }

        EditorGUILayout.Space();

        showBehaviours = EditorGUILayout.Foldout(showBehaviours, "Behaviours", true, EditorStyles.foldout);
        if (showBehaviours)
        {
            EditorGUILayout.PropertyField(rotatesToWaypoint);
            EditorGUILayout.PropertyField(patrolOnStart);
        }

        EditorGUILayout.Space();

        showParameters = EditorGUILayout.Foldout(showParameters, "Parameters", true, EditorStyles.foldout);
        if (showParameters)
        {
            EditorGUILayout.PropertyField(speed);

            if (rotatesToWaypoint.boolValue)
                EditorGUILayout.PropertyField(rotationAnglePerSecond);
            else
                EditorGUILayout.PropertyField(waitTime);
        }

        EditorGUILayout.Space();

        showEvents = EditorGUILayout.Foldout(showEvents, "Events", true, EditorStyles.foldout);
        if (showEvents)
        {
            EditorGUILayout.PropertyField(OnMovementStart);
            EditorGUILayout.PropertyField(OnMovementEnd);
            EditorGUILayout.PropertyField(OnWaypointReached);
            EditorGUILayout.PropertyField(OnPathFinished);
        }

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }

}