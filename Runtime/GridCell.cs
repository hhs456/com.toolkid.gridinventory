using UnityEngine;
using UnityEngine.UI;
namespace Toolkid.UIGrid {
    public class GridData {
        [SerializeField] int m_ObjectID;
        [SerializeField] RawImage m_Skin;
        [SerializeField] Vector2Int m_NativeCell;
        [SerializeField] Vector2Int m_InventoryIndex;

        public GridData(RawImage skin) {
            this.Skin = skin;
        }

        public int ObjectID { get => m_ObjectID; set => m_ObjectID = value; }
        public RawImage Skin { get => m_Skin; set => m_Skin = value; }
        public Vector2Int NativeCell { get => m_NativeCell; set => m_NativeCell = value; }
        public Vector2Int InventoryIndex { get => m_InventoryIndex; set => m_InventoryIndex = value; }

        public void SetCell(Vector2Int cell) {
            this.NativeCell = cell;
        }
        public void SetSkin(bool enable) {
            Skin.enabled = enable;
        }
    }
}