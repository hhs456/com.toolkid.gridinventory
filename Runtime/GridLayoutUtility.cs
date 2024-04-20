using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.GridLayoutGroup;

namespace Toolkid.UIGrid {
    public static class GridLayoutUtility {

        public static Vector2Int GetIndex(this Vector2Int centerIndex, Vector2Int relativeIndex, Corner startCorner) {
            int x = -1;
            int y = 0;
            switch (startCorner) {
                case Corner.UpperLeft:
                    x = centerIndex.x + relativeIndex.x;
                    y = centerIndex.y - relativeIndex.y;
                    break;
                case Corner.UpperRight:
                    x = centerIndex.x - relativeIndex.x;
                    y = centerIndex.y - relativeIndex.y;
                    break;
                case Corner.LowerLeft:
                    x = centerIndex.x + relativeIndex.x;
                    y = centerIndex.y + relativeIndex.y;
                    break;
                case Corner.LowerRight:
                    x = centerIndex.x - relativeIndex.x;
                    y = centerIndex.y + relativeIndex.y;
                    break;
                default:
                    break;
            }
            return new Vector2Int(x, y);
        }

        public static int ToInt(this Vector2Int index, int columnCount) {
            return index.y * columnCount + index.x;
        }
        public static int ToInt(GridRegion gridSystem, Vector2Int centerIndex, Vector2Int relativeIndex) {
            return centerIndex.GetIndex(relativeIndex, gridSystem.StartCorner).ToInt(gridSystem.GridCount.x);
        }
        public static Vector2Int ToVector2(this int index, int columnCount) {           
            int offset = columnCount - 1;
            return new Vector2Int(index % offset, index / offset);
        }
        public static Vector2Int RotateClockwise(this Vector2Int index, int size) {
            int newX = size - 1 - index.y;
            int newY = index.x;
            return new Vector2Int(newX, newY);
        }
        public static Vector2Int RotateClockwise(this Vector2Int cell) {
            int newX = -cell.y;
            int newY = cell.x;
            return new Vector2Int(newX, newY);
        }
    }
}