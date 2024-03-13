using UnityEngine;
using UnityEngine.UI;
namespace Toolkid.UIGrid {
    public class GridCell {
        public RawImage skin;        
        public Vector2Int nativeCell;
        public Vector2Int inventoryIndex;

        public GridCell(RawImage skin) {
            this.skin = skin;
        }
        public void SetCell(Vector2Int cell) {
            this.nativeCell = cell;
        }
        public void SetSkin(bool enable) {
            skin.enabled = enable;
        }
    }
}