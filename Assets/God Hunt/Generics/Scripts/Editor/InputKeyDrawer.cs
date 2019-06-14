//using UnityEngine;
//using UnityEditor;

//[CustomPropertyDrawer(typeof(InputKey))]
//public class InputKeyDrawer : PropertyDrawer
//{
//    const float lineHeight = 16f;
//    const float borderHeight = 2f;
//    const float borderWidth = 2f;
//    const float greyScaleValue = 1 / 255 * 150;

//    int linesCount = 1;

//    Rect myRect;

//    Color myGrayScale = new Color(.5f, .5f, .5f);

//    bool showClass = false;

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        #region general settings
//        EditorStyles.foldout.fontStyle = FontStyle.Bold;
//        #endregion

//        label = EditorGUI.BeginProperty(position, label, property);

//        myRect = new Rect(position.position, position.size);
//        myRect.xMin += EditorGUI.indentLevel * 15;

//        #region draw rect and add border
//        EditorGUIHelper.DrawRect(myRect, Color.black);

//        myRect.xMin += borderWidth;
//        myRect.yMin += borderHeight;
//        myRect.xMax -= borderWidth;
//        myRect.yMax -= borderHeight;

//        EditorGUIHelper.DrawRect(myRect, myGrayScale);
//        #endregion

//        showClass = EditorGUI.Foldout(new Rect(myRect.position, new Vector2(myRect.width, lineHeight)), showClass, label, EditorStyles.foldout);

//        if (showClass)
//        {
//            myRect = EditorGUI.IndentedRect(myRect);

//            var Key = DisplayField("Key", property, true);

//            var isAxis = property.FindPropertyRelative("isAxis");
//            var keyString = property.FindPropertyRelative("keyString");

//            switch (Key.enumValueIndex)
//            {
//                case (int)InputKeyButton.Horizontal:
//                    isAxis.boolValue = true;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Horizontal";
//                    break;
//                case (int)InputKeyButton.Vertical:
//                    isAxis.boolValue = true;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Vertrical";
//                    break;
//                case (int)InputKeyButton.Pause:
//                    isAxis.boolValue = false;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Pause";
//                    break;
//                case (int)InputKeyButton.Map:
//                    isAxis.boolValue = false;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Map";
//                    break;
//                case (int)InputKeyButton.Inventory:
//                    isAxis.boolValue = false;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Inventory";
//                    break;
//                case (int)InputKeyButton.Jump:
//                    isAxis.boolValue = false;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Jump";
//                    break;
//                case (int)InputKeyButton.Attack:
//                    isAxis.boolValue = false;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Attack";
//                    break;
//                case (int)InputKeyButton.Dash:
//                    isAxis.boolValue = false;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Dash";
//                    break;
//                case (int)InputKeyButton.Run:
//                    isAxis.boolValue = false;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Run";
//                    break;
//                case (int)InputKeyButton.Submit:
//                    isAxis.boolValue = false;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Submit";
//                    break;
//                case (int)InputKeyButton.Cancel:
//                    isAxis.boolValue = false;
//                    keyString.stringValue = TestInputManager.CurrentInputDevice + "Cancel";
//                    break;
//            }


//            if (isAxis.boolValue)
//            {
//                var Treshold = DisplayField("Treshold", property, true);

//                var TriggerMode = DisplayField("TriggerMode", property, true);
//            }
//            else
//            {
//                var Interaction = DisplayField("Interaction", property, true);

//                if (Interaction.enumValueIndex == (int)InputType.Hold)
//                {
//                    var TriggerWhileHolding = DisplayField("TriggerWhileHolding", property, true);

//                    if (TriggerWhileHolding.boolValue)
//                    {
//                        var TimeToHold = DisplayField("TimeToHold", property, true);
//                    }
//                }
//            }
//        }

//        property.serializedObject.ApplyModifiedProperties();

//        EditorGUI.EndProperty();
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return GetHeight(
//            property.FindPropertyRelative("isAxis").boolValue,
//            (InputType)property.FindPropertyRelative("Interaction").enumValueIndex,
//            property.FindPropertyRelative("TriggerWhileHolding").boolValue
//            );
//    }

//    float GetHeight(bool _isAxis, InputType _type, bool _triggerWhileHolding)
//    {
//        if (showClass)
//        {
//            if (_isAxis)
//            {
//                linesCount = 4;
//            }
//            else
//            {
//                linesCount = 3;
//                if (_type == InputType.Hold)
//                {
//                    linesCount = 4;
//                    if (_triggerWhileHolding)
//                        linesCount = 5;
//                }
//            }
//        }
//        else
//        {
//            linesCount = 1;
//        }


//        return borderHeight * 2 + linesCount * (lineHeight + borderHeight);
//    }

//    SerializedProperty DisplayField(string _fieldName, SerializedProperty _property, bool _newLine = false)
//    {
//        if (_newLine)
//            myRect.y += 18f;
//        var field = _property.FindPropertyRelative(_fieldName);
//        EditorGUI.PropertyField(new Rect(myRect.position, new Vector2(myRect.width, lineHeight)), field);
//        return field;
//    }

//}