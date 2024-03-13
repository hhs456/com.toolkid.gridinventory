using UnityEditor;
using UnityEngine;
namespace Toolkid.UIGrid {
    [CustomEditor(typeof(GridSystem))]
    public class GridSystemEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Resize Immediately")) {
                ((GridSystem)target).Initialize();                
            }
        }
    }
}