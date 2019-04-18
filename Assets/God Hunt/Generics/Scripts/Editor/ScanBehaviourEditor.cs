using UnityEditor;

[CustomEditor(typeof(ScanBehaviour))]
[CanEditMultipleObjects]
public class ScanBehaviourEditor : Editor
{
    SerializedProperty timeToScan, fovAngle, scanAreaLenght, obstacleLayer, OnTargetSpotted,
        OnTargetLost, scanType, spotLightColor, detectedSpotlightColor, scanAreaRadius, canSeeThroughObstacles, previewWithSpotlight, scanTarget;

    bool showParameters = true, showEvents = true, showBehaviours = true;

    private void OnEnable()
    {
        timeToScan = serializedObject.FindProperty("timeToScan");
        fovAngle = serializedObject.FindProperty("fovAngle");
        scanAreaLenght = serializedObject.FindProperty("scanAreaLenght");
        obstacleLayer = serializedObject.FindProperty("obstacleLayer");
        OnTargetSpotted = serializedObject.FindProperty("OnTargetSpotted");
        OnTargetLost = serializedObject.FindProperty("OnTargetLost");
        scanType = serializedObject.FindProperty("scanType");
        spotLightColor = serializedObject.FindProperty("spotLightColor");
        detectedSpotlightColor = serializedObject.FindProperty("detectedSpotlightColor");
        scanAreaRadius = serializedObject.FindProperty("scanAreaRadius");
        canSeeThroughObstacles = serializedObject.FindProperty("canSeeThroughObstacles");
        previewWithSpotlight = serializedObject.FindProperty("previewWithSpotlight");
        scanTarget = serializedObject.FindProperty("scanTarget");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorStyles.foldout.fontStyle = UnityEngine.FontStyle.Bold;

        EditorGUILayout.Space();

        showBehaviours = EditorGUILayout.Foldout(showBehaviours, "Behaviours", true, EditorStyles.foldout);
        if (showBehaviours)
        {
            EditorGUILayout.PropertyField(scanType);
            EditorGUILayout.PropertyField(canSeeThroughObstacles);
            if(scanType.enumValueIndex == (int)ScanBehaviour.ScanType.fieldOfView)
                EditorGUILayout.PropertyField(previewWithSpotlight);
        }

        EditorGUILayout.Space();

        showParameters = EditorGUILayout.Foldout(showParameters, "Parameters", true, EditorStyles.foldout);
        if (showParameters)
        {
            EditorGUILayout.PropertyField(scanTarget);
            EditorGUILayout.PropertyField(timeToScan);

            if (scanType.enumValueIndex == (int)ScanBehaviour.ScanType.fieldOfView)
            {
                EditorGUILayout.PropertyField(fovAngle);
                EditorGUILayout.PropertyField(scanAreaLenght);
                if (previewWithSpotlight.boolValue)
                {
                    EditorGUILayout.PropertyField(spotLightColor);
                    EditorGUILayout.PropertyField(detectedSpotlightColor);
                }
            }
            else if (scanType.enumValueIndex == (int)ScanBehaviour.ScanType.circularArea)
            {
                EditorGUILayout.PropertyField(scanAreaRadius);
            }
            if (!canSeeThroughObstacles.boolValue)
                EditorGUILayout.PropertyField(obstacleLayer);
        }

        EditorGUILayout.Space();

        showEvents = EditorGUILayout.Foldout(showEvents, "Events", true, EditorStyles.foldout);
        if (showEvents)
        {
            EditorGUILayout.PropertyField(OnTargetSpotted);
            EditorGUILayout.PropertyField(OnTargetLost);
        }

        serializedObject.ApplyModifiedProperties();
    }

}