using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Toolkid.GridInventory {
    public static class GridLayoutUtility {
        public static int ToInt(this Vector2Int index, Vector2Int cell, Corner startCorner, int columnCount) {
            int x = -1;
            int y = 0;
            switch (startCorner) {
                case Corner.UpperLeft:
                    x = index.x + cell.x;
                    y = index.y - cell.y;
                    break;
                case Corner.UpperRight:
                    x = index.x - cell.x;
                    y = index.y - cell.y;
                    break;
                case Corner.LowerLeft:
                    x = index.x + cell.x;
                    y = index.y + cell.y;
                    break;
                case Corner.LowerRight:
                    x = index.x - cell.x;
                    y = index.y + cell.y;
                    break;
                default:
                    break;
            }
            return y * columnCount + x;
        }
        public static int ToInt(GridSystem gridSystem, Vector2Int index, Vector2Int cell) {
            return index.ToInt(cell, gridSystem.StartCorner, gridSystem.GridCount.x);
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
            int newX = cell.y;
            int newY = -cell.x;
            return new Vector2Int(newX, newY);
        }
    }
}