using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Toolkid.UIGrid {
    /// <summary>
    /// Manages grid-based calculations and transitions using Unity's Grid system.
    /// </summary>
    public class GridRegion : MonoBehaviour {

        [SerializeField] private RectTransform rect;
        [SerializeField] private Grid grid;
        [SerializeField] private Material material;
        [SerializeField] private Vector2Int gridCount = new Vector2Int(6, 6);
        [SerializeField] private Corner startCorner;

        [SerializeField, Tooltip("Experimental function.")] private Vector2Int positionOffset;

        private bool isTileTexture = false;        

        public RectTransform Rect { get => rect; }
        public Grid Grid { get => grid; }
        public Vector2Int PositionOffset { get => positionOffset; }
        public Vector2Int GridCount { get => gridCount; }
        public Corner StartCorner { get => startCorner; set => startCorner = value; }
        

        private void OnValidate() {
            if (material) {
                material.SetVector("_GridCount", new Vector4(gridCount.x, gridCount.y, 0, 0));
                material.SetInt("_IsTile", isTileTexture ? 1 : 0);
            }
        }

        /// <summary>
        /// Determines whether the <see cref="GridRegion"/> contains the specified grid index.
        /// </summary>
        /// <param name="index">The grid index to check.</param>
        /// <returns>True if the <see cref="GridRegion"/> contains the specified index, otherwise false.</returns>
        public bool Contains(Vector2Int index) {
            bool withinXBounds = index.x >= 0 && index.x < GridCount.x;
            bool withinYBounds = index.y >= 0 && index.y < GridCount.y;
            return withinXBounds && withinYBounds;
        }

        /// <summary>
        /// Calculates the grid index based on the center point and relative position.
        /// </summary>
        /// <param name="center">The center point of the grid.</param>
        /// <param name="relative">The position relative to the center point.</param>
        /// <returns>The computed grid index.</returns>
        public Vector2Int GetIndex(Vector2Int center, Vector2Int relative) {
            int x = center.x + relative.x;
            int y = center.y + relative.y;
            switch (startCorner) {
                case Corner.UpperLeft:
                    y = center.y - relative.y;
                    break;
                case Corner.UpperRight:
                    y = center.y - relative.y;
                    x = center.x - relative.x;
                    break;
                case Corner.LowerLeft:
                    break;
                case Corner.LowerRight:
                    x = center.x - relative.x;
                    break;
                default:
                    break;
            }
            return new Vector2Int(x, y);
        }

        /// <summary>
        /// Gets the order value for the specified index and relative position.
        /// </summary>
        /// <param name="index">The index of the grid.</param>
        /// <param name="relative">The relative position of the cell.</param>
        /// <returns>The order value.</returns>
        public int GetOrder(Vector2Int index, Vector2Int relative) {
            return index.GetIndex(relative, startCorner).ToInt(GridCount.x);
        }
        
        /// <summary>
        /// Initializes the grid system using the previously set grid count.
        /// </summary>
        public void Initializes() {
            Initializes(gridCount);
        }

        /// <summary>
        /// Initializes the grid system with the specified grid count and adjusts various properties accordingly.
        /// </summary>
        /// <param name="gridCount">The number of grids in the system.</param>
        public void Initializes(Vector2Int gridCount) {
            this.gridCount = gridCount;
            if (material) {
                material.SetVector("_GridCount", new Vector4(gridCount.x, gridCount.y, 0, 0));
                material.SetInt("_IsTile", isTileTexture ? 1 : 0);
            }
            positionOffset = new Vector2Int(gridCount.x / 2, gridCount.y / 2);
            Vector2 canvasSize = Rect.rect.size;
            Grid.cellSize = new Vector3(canvasSize.x / gridCount.x, canvasSize.y / gridCount.y, 0);
            float x_offset = gridCount.x % 2 == 0 ? 0 : Grid.cellSize.x / 2;
            float y_offset = gridCount.y % 2 == 0 ? 0 : Grid.cellSize.y / 2;
            switch (startCorner) {
                case Corner.UpperLeft:
                    x_offset = -x_offset;
                    break;
                case Corner.UpperRight:
                    break;
                case Corner.LowerLeft:
                    x_offset = -x_offset;
                    y_offset = -y_offset;
                    break;
                case Corner.LowerRight:
                    y_offset = -y_offset;
                    break;
                default:
                    break;
            }
            grid.transform.localPosition = new Vector3(grid.transform.localPosition.x + x_offset, grid.transform.localPosition.y + y_offset, 0);
        }

        /// <summary>
        /// Converts a world space position to the center position of the corresponding grid cell in world space.
        /// </summary>
        /// <param name="position">The world space position to convert.</param>
        /// <returns>The center position of the corresponding grid cell in world space.</returns>
        public Vector3 GetCellCenterWorld(Vector3 position) {
            Vector2Int gridIndex = GetIndex(position);            
            return GetWorldPosition(gridIndex);
        }

        /// <summary>
        /// Converts a grid cell position to the corresponding world space position.
        /// </summary>
        /// <param name="cell">The position of the grid cell to convert.</param>
        /// <returns>The world space position corresponding to the center of the grid cell.</returns>
        public Vector3 GetWorldPosition(Vector2Int cell) {
            int x = cell.x - PositionOffset.x;
            int y = cell.y - PositionOffset.y;
            switch (startCorner) {
                case Corner.UpperLeft:
                    y = -y - 1;
                    break;
                case Corner.UpperRight:
                    x = -x - 1;
                    y = -y - 1;
                    break;
                case Corner.LowerLeft:
                    break;
                case Corner.LowerRight:
                    x = -x - 1;
                    break;
                default:
                    break;
            }
            Vector3Int placePos = new Vector3Int(x, y, 0);
            return Grid.GetCellCenterWorld(placePos);
        }

        /// <summary>
        /// Converts a world space position to the corresponding grid cell index.
        /// </summary>
        /// <param name="position">The world space position to convert.</param>
        /// <returns>The index of the grid cell corresponding to the provided world space position.</returns>
        public Vector2Int GetIndex(Vector3 position) {
            Vector3Int cellPos = Grid.WorldToCell(position);
            int x = cellPos.x + PositionOffset.x;            
            int y = cellPos.y + PositionOffset.y;            
            switch (startCorner) {
                case Corner.UpperLeft:
                    y = GridCount.y - y - 1;
                    break;
                case Corner.UpperRight:
                    x = GridCount.x - x - 1;
                    y = GridCount.y - y - 1;
                    break;
                case Corner.LowerLeft:
                    break;
                case Corner.LowerRight:
                    x = GridCount.x - x - 1;
                    break;
                default:
                    break;
            }
            return new Vector2Int(x, y);            
        }
    }
}