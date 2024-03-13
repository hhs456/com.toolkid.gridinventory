using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Toolkid.UIGrid {
    public class GridSystem : MonoBehaviour {
        public RectTransform Rect { get => rect; }
        public Grid Grid { get => grid; }
        public Vector2Int PositionOffset { get => m_PositionOffset; }
        public Vector2Int GridCount { get => m_GridCount; }
        public Corner StartCorner { get => startCorner; set => startCorner = value; }

        [SerializeField] private RectTransform rect;
        [SerializeField] private Grid grid;
        [SerializeField] private Vector2Int m_GridCount = new Vector2Int(6, 6);

        [SerializeField] private Corner startCorner;

        private Vector2Int m_PositionOffset;        

        public bool TryArea(Vector2Int index) {
            bool isOutArea = false;
            if (index.y >= GridCount.y) {
                isOutArea = true;
            }
            else if (index.y < 0) {
                isOutArea = true;
            }
            if (index.x >= GridCount.x) {
                isOutArea = true;
            }
            else if (index.x < 0) {
                isOutArea = true;
            }
            return isOutArea;
        }

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

        public int GetOrder(Vector2Int index, Vector2Int cell) {
            return index.ToInt(cell, startCorner, GridCount.x);
        }

        public void Initialize() {            
            m_PositionOffset = new Vector2Int(GridCount.x / 2, GridCount.y / 2);
            Vector2 canvasSize = Rect.rect.size;
            Grid.cellSize = new Vector3(canvasSize.x / GridCount.x, canvasSize.y / GridCount.y, 0);
            float x_offset = GridCount.x % 2 == 0 ? 0 : Grid.cellSize.x / 2;            
            float y_offset = GridCount.y % 2 == 0 ? 0 : Grid.cellSize.y / 2;            
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

        public Vector3 GetCellCenterWorld(Vector3 position) {
            Vector2Int gridIndex = GetIndex(position);            
            return GetWorldPosition(gridIndex);
        }

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