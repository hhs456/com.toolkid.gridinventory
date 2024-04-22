using UnityEngine;

namespace Toolkid.UIGrid {
    public static class GridExtension {
        /// <summary>
        /// Gets the 2D size of the grid.
        /// </summary>
        /// <param name="grid">The grid to get the size from.</param>
        /// <returns>The 2D size of the grid.</returns>
        public static Vector2 Get2DSize(this Grid grid) {
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
}