using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
namespace Toolkid.UIGrid {
    [Serializable]
    public class GridSlotData {        
        public RawImage Skin { get => skin; set => skin = value; }
        public Vector2Int NativeCell { get => nativeCell; set => nativeCell = value; }
        public Vector2Int ArrayIndex { get => arrayIndex; set => arrayIndex = value; }
        
        [SerializeField] private RawImage skin;        
        [SerializeField] private Vector2Int nativeCell;
        [SerializeField, FormerlySerializedAs("inventoryIndex")] private Vector2Int arrayIndex;        

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