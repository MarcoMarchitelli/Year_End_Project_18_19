using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[CustomPropertyDrawer(typeof(CollisionDataDrawer))]
public class CollisionDataDrawer : PropertyDrawer
{
    const float lineHeight = 16f;
    const float borderHeight = 2f;
    const float borderWidth = 2f;
    const float greyScaleValue = 1 / 255 * 150;

    int linesCount = 1;

    Rect myRect;

    Color myGrayScale = new Color(.5f, .5f, .5f);

    bool showClass = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        #region general settings
        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        #endregion

        CollisionData c = fieldInfo.GetValue( property.serializedObject.targetObject ) as CollisionData;

        Debug.Log(c);

        label = EditorGUI.BeginProperty(position, label, property);

        myRect = new Rect(position.position, position.size);
        myRect.xMin += EditorGUI.indentLevel * 15;

        #region draw rect and add border
        EditorGUIHelper.DrawRect(myRect, Color.black);

        myRect.xMin += borderWidth;
        myRect.yMin += borderHeight;
        myRect.xMax -= borderWidth;
        myRect.yMax -= borderHeight;

        EditorGUIHelper.DrawRect(myRect, myGrayScale);
        #endregion

        showClass = EditorGUI.Foldout(new Rect(myRect.position, new Vector2(myRect.width, lineHeight)), showClass, label, EditorStyles.foldout);

        if (showClass)
        {
            myRect = EditorGUI.IndentedRect(myRect);

            DisplayField("collisionType", property, true);

            var eventType = DisplayField("eventType", property, true);

            var searchFor = DisplayField("searchFor", property, true);

            switch (searchFor.enumValueIndex)
            {
                case 0:
                    DisplayField("component", property, true);
                    switch (eventType.enumValueIndex)
                    {
                        case 0:
                            DisplayField("onComponentCollisionEnter", property, true);
                            break;
                        case 1:
                            DisplayField("onComponentCollisionStay", property, true);
                            break;
                        case 2:
                            DisplayField("onComponentCollisionExit", property, true);
                            break;
                    }
                    break;
                case 1:
                    DisplayField("layer", property, true);
                    switch (eventType.enumValueIndex)
                    {
                        case 0:
                            DisplayField("onLayerCollisionEnter", property, true);
                            break;
                        case 1:
                            DisplayField("onLayerCollisionStay", property, true);
                            break;
                        case 2:
                            DisplayField("onLayerCollisionExit", property, true);
                            break;
                    }
                    break;
                case 2:
                    DisplayField("tag", property, true);
                    switch (eventType.enumValueIndex)
                    {
                        case 0:
                            DisplayField("onTagCollisionEnter", property, true);
                            break;
                        case 1:
                            DisplayField("onTagCollisionStay", property, true);
                            break;
                        case 2:
                            DisplayField("onTagCollisionExit", property, true);
                            break;
                    }
                    break;
            }

        }

        property.serializedObject.ApplyModifiedProperties();

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return GetHeight( property );
    }

    float GetHeight( SerializedProperty _property )
    {
        if (showClass)
        {
            linesCount = 10;
        }
        else
        {
            linesCount = 1;
        }


        return borderHeight * 2 + linesCount * (lineHeight + borderHeight);
    }

    SerializedProperty DisplayField(string _fieldName, SerializedProperty _property, bool _newLine = false)
    {
        if (_newLine)
            myRect.y += 18f;
        var field = _property.FindPropertyRelative(_fieldName);
        EditorGUI.PropertyField(new Rect(myRect.position, new Vector2(myRect.width, lineHeight)), field);
        return field;
    }

}