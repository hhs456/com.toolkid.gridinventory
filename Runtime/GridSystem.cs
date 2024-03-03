using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Toolkid.GridInventory {
    public class GridSystem : MonoBehaviour {
        public RectTransform Rect { get => rect; }
        public Grid Grid { get => grid; }
        public Vector2Int PositionOffset { get => m_PositionOffset; }
        public Vector2Int GridCount { get => m_GridCount; }

        [SerializeField] private RectTransform rect;
        [SerializeField] private Grid grid;
        [SerializeField] private Vector2Int m_GridCount = new Vector2Int(6, 6);

        public bool enableOriginTop = false;
        public bool enableOriginRight = false;

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

        public Vector2Int GetIndex(Vector2Int index, Vector2Int cell) {
            int horizontalPosition = enableOriginRight ? index.x - cell.x : index.x + cell.x;
            int verticalPosition = enableOriginTop ? index.y - cell.y : index.y + cell.y;

            return new Vector2Int(horizontalPosition, verticalPosition);
        }

        public int GetOrder(Vector2Int index, Vector2Int cell) {
            int horizontalPosition = enableOriginRight ? index.x - cell.x : index.x + cell.x;
            int verticalPosition = enableOriginTop ? index.y - cell.y : index.y + cell.y;

            return verticalPosition * GridCount.x + horizontalPosition;
        }

        public void Initialize() {            
            m_PositionOffset = new Vector2Int(GridCount.x / 2, GridCount.y / 2);
            Vector2 canvasSize = Rect.rect.size;
            Grid.cellSize = new Vector3(canvasSize.x / GridCount.x, canvasSize.y / GridCount.y, 0);            
        }

        public Vector3 GetCellCenterWorld(Vector3 position) {
            Vector2Int gridIndex = GetIndex(position);
            int horizontalPosition = gridIndex.x - PositionOffset.x;
            horizontalPosition = enableOriginRight ? -(gridIndex.x - PositionOffset.x) - 1 : horizontalPosition;
            int verticalPosition = gridIndex.y - PositionOffset.y;
            verticalPosition = enableOriginTop ? -(gridIndex.y - PositionOffset.y) - 1 : verticalPosition;

            Vector3Int placePos = new Vector3Int(horizontalPosition, verticalPosition, 0);
            return Grid.GetCellCenterWorld(placePos);
        }

        public Vector3 GetWorldPosition(Vector2Int cell) {
            int horizontalPosition = cell.x - PositionOffset.x;
            horizontalPosition = enableOriginRight ? -(cell.x - PositionOffset.x) - 1 : horizontalPosition;
            int verticalPosition = cell.y - PositionOffset.y;
            verticalPosition = enableOriginTop ? -(cell.y - PositionOffset.y) - 1 : verticalPosition;

            Vector3Int placePos = new Vector3Int(horizontalPosition, verticalPosition, 0);
            return Grid.GetCellCenterWorld(placePos);
        }

        public Vector2Int GetIndex(Vector3 position) {
            Vector3Int cellPos = Grid.WorldToCell(position);
            int horizontalPosition = cellPos.x + PositionOffset.x;
            if (enableOriginRight) {
                horizontalPosition = GridCount.x - horizontalPosition - 1;
            }
            int verticalPosition = cellPos.y + PositionOffset.y;
            if (enableOriginTop) {
                verticalPosition = GridCount.y - verticalPosition - 1;
            }
            return new Vector2Int(horizontalPosition, verticalPosition);            
        }
    }
}