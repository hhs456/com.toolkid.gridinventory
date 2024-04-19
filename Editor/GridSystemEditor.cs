using UnityEditor;
using UnityEngine;
namespace Toolkid.UIGrid {
    [CustomEditor(typeof(GridRegion))]
    public class GridSystemEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Resize Immediately")) {
                ((GridRegion)target).Initializes();                
            }
        }
    }
}