
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(GridSystem))]
public class GridSystemEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Setup")) {
            ((GridSystem)target).Initialize();
            ((GridSystem)target).Setup();
        }
    }
}
