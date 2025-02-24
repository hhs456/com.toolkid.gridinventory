using System;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Toolkid.UIGrid {
    /// <summary>
    /// Manages grid-based calculations and transitions using Unity's Grid system.
    /// </summary>
    public class GridCursor : MonoBehaviour {
        [SerializeField] private GridRegion benchmark;
        public event EventHandler<Vector2Int> Clicked;
        private void Update() {
            if (Input.GetMouseButtonUp(0)) {
                RectTransformUtility.ScreenPointToWorldPointInRectangle(benchmark.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out Vector3 cursorPos);
                Vector2Int gridIndex = benchmark.GetIndex(cursorPos);
                Clicked?.Invoke(this, gridIndex);                
            }
        }
    }
}