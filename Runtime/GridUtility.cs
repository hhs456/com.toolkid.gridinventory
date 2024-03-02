using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtility {
    public static Vector2 GetSizeInVector2(this Grid grid) {
        switch (grid.cellSwizzle) {
            case GridLayout.CellSwizzle.XYZ:
                return new Vector2(grid.cellSize.x, grid.cellSize.y);
            case GridLayout.CellSwizzle.XZY:
                return new Vector2(grid.cellSize.x, grid.cellSize.z);
            case GridLayout.CellSwizzle.YXZ:
                return new Vector2(grid.cellSize.y, grid.cellSize.x);
            case GridLayout.CellSwizzle.YZX:
                return new Vector2(grid.cellSize.y, grid.cellSize.z);
            case GridLayout.CellSwizzle.ZXY:
                return new Vector2(grid.cellSize.z, grid.cellSize.x);
            case GridLayout.CellSwizzle.ZYX:
                return new Vector2(grid.cellSize.z, grid.cellSize.y);
        }
        return Vector2.zero;
    }
}
