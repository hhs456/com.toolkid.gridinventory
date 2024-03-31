using UnityEditor;
using UnityEngine;
namespace Toolkid.UIGrid {
    [CustomEditor(typeof(StackablesInventory))]
    public class StackablesInventoryEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Prev Page")) {
                ((StackablesInventory)target).PrevPage();
            }
            if (GUILayout.Button("Next Page")) {
                ((StackablesInventory)target).NextPage();                
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}