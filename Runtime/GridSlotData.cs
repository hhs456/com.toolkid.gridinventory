using UnityEngine;
using UnityEngine.UI;
namespace Toolkid.UIGrid {
    public class GridSlotData {

        [SerializeField] private int objectID;
        [SerializeField] private RawImage skin;
        [SerializeField] private Vector2Int nativeCell;
        [SerializeField] private Vector2Int inventoryIndex;

        public int ObjectID { get => objectID; set => objectID = value; }
        public RawImage Skin { get => skin; set => skin = value; }
        public Vector2Int NativeCell { get => nativeCell; set => nativeCell = value; }
        public Vector2Int InventoryIndex { get => inventoryIndex; set => inventoryIndex = value; }

        public GridSlotData(RawImage skin) {
            Skin = skin;
        }

        public void SetCell(Vector2Int cell) {
            NativeCell = cell;
        }
        public void SetSkin(bool enable) {
            Skin.enabled = enable;
        }
    }
}