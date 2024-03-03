using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PlaceablesData))]
public class PlaceablesDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var arect = position;
        arect.width = EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth;        
        arect.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(arect, property.FindPropertyRelative("m_ID"), new GUIContent("Object ID"));
        arect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
        EditorGUI.LabelField(arect, new GUIContent("Sharp"));
        arect.x += EditorGUIUtility.labelWidth;
        arect.width = 24f;
        arect.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;        
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {                
                SerializedProperty slot = property.FindPropertyRelative("m_Sharp").GetArrayElementAtIndex(i * 5 + j);
                if(i * 5 + j == 12) {
                    slot.boolValue = true;
                }
                if (slot.boolValue) {
                    GUI.backgroundColor = Color.green;
                }
                else {
                    GUI.backgroundColor = Color.white;
                }
                slot.boolValue = EditorGUI.Toggle(arect, new GUIContent(""), slot.boolValue, EditorStyles.toolbarButton);
                arect.x += 24f + EditorGUIUtility.standardVerticalSpacing;
            }
            arect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 3f;
            arect.x = position.x + EditorGUIUtility.labelWidth;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, label) + 3 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f);
    }
}
