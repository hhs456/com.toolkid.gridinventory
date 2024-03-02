using UnityEngine;
using UnityEngine.UI;
namespace Toolkid.GridInventory {
    public class GridCell {
        public Image skin;
        public Vector2Int position;

        public GridCell(Image skin) {
            this.skin = skin;
        }
        public void SetCell(Vector2Int cell) {
            this.position = cell;
        }
        public void SetSkin(bool enable) {
            skin.enabled = enable;
        }
    }
}