using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Toolkid.UIGrid {
    [CustomPropertyDrawer(typeof(PlaceablesData))]
    public class PlaceablesDataDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var arect = position;
            arect.height = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded = EditorGUI.Foldout(arect, property.isExpanded, label)) {
                arect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
                //arect.width = EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth;
                EditorGUI.PropertyField(arect, property.FindPropertyRelative("identifier"), new GUIContent("Item ID"));
                //arect.width *= 2f;
                arect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
                EditorGUI.PropertyField(arect, property.FindPropertyRelative("name"), new GUIContent("Name"));
                arect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
                EditorGUI.PropertyField(arect, property.FindPropertyRelative("tooltip"), new GUIContent("Tooltip"));
                arect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
                EditorGUI.LabelField(arect, new GUIContent("Sharp"));
                arect.x += EditorGUIUtility.labelWidth;
                arect.width = 24f;
                arect.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
                property.FindPropertyRelative("sharp").arraySize = 25;
                if (property.propertyPath[property.propertyPath.Length - 1] == ']') {
                    EditorGUI.indentLevel--;
                }
                for (int i = 0; i < 5; i++) {
                    for (int j = 0; j < 5; j++) {
                        SerializedProperty slot = property.FindPropertyRelative("sharp").GetArrayElementAtIndex(i * 5 + j);
                        if (i * 5 + j == 12) {
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
                if (property.propertyPath[property.propertyPath.Length - 1] == ']') {
                    EditorGUI.indentLevel++;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return property.isExpanded ? EditorGUI.GetPropertyHeight(property, label) + 5 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f) : EditorGUI.GetPropertyHeight(property, label);
        }
    }
}
