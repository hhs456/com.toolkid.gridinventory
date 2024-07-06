using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Toolkid.UIGrid {
    /// <summary>
    /// Manages the validation and placement of grid-based objects in the Unity environment.
    /// </summary>
    public class GridValidator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private InventoryManager inventoryManager;
        [FormerlySerializedAs("gridSystem") , SerializeField] private GridRegion gridRegion;
        [SerializeField] private int sharpSize = 5;
        [SerializeField] private Vector2Int center;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject[] relatives;
        [SerializeField] private bool isValid = false;
        public GameObject Prefab { get => prefab; set => prefab = value; }
        public Vector2Int Center { get => center; set => center = value; }

        public bool IsValid { get => isValid; }

        [SerializeField] public readonly List<GridSlotData> gridDatas = new List<GridSlotData>();
                
        private bool isHovering = false;

        public event EventHandler<SlotData> Validated;
        public event EventHandler<SlotData> Invalidated;        

        /// <summary>
        /// Initializes the grid validator component.
        /// </summary>
        public void Initializes() {
            GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridRegion.Grid.cellSize;
        }

        /// <summary>
        /// Generates a preview of the grid based on the provided sharp array.
        /// </summary>
        /// <param name="Sharp">An array indicating the shape of the grid.</param>
        public void Preview(bool[] Sharp) {
            gridDatas.Clear();
            GetComponent<Grid>().cellSize = InventoryManager.Current.GridRegion.Grid.cellSize;
            for (int i = 0; i < 25; i++) {
                if (Sharp[i]) {
                    gridDatas.Add(new GridSlotData(Instantiate(prefab).GetComponent<RawImage>()));
                    int lastIndex = gridDatas.Count - 1;
                    gridDatas[lastIndex].Skin.color = Color.clear; // Detecting pointer only
                    gridDatas[lastIndex].Skin.transform.localPosition = Vector3.zero;
                    gridDatas[lastIndex].Skin.GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridRegion.Grid.Get2DSize();
                    int midtern = sharpSize * sharpSize / 2;
                    Vector2Int cell = new Vector2Int((i % sharpSize - midtern % sharpSize), -(i / sharpSize - midtern / sharpSize));
                    gridDatas[lastIndex].SetCell(cell);
                    gridDatas[lastIndex].Skin.transform.SetParent(InventoryManager.Current.Validator.transform);
                    gridDatas[lastIndex].Skin.transform.localPosition = InventoryManager.Current.Validator.GetComponent<Grid>().CellToLocal(new Vector3Int(cell.x, cell.y, 0));
                    gridDatas[lastIndex].Skin.transform.localScale = Vector3.one;
                }
            }
        }

        /// <summary>
        /// Places the grid validator on the specified grid index.
        /// </summary>
        /// <param name="index">The index where the grid validator will be placed.</param>
        public void PlaceOn(Vector2Int index) {
            Center = index;
            foreach (GridSlotData cell in gridDatas) {                
                cell.InventoryIndex = gridRegion.GetIndex(index, cell.NativeCell);
            }
            var anyClick = new GlobalClickDetector<GridValidator>(this, d => !d.isHovering, TryCancel);
            anyClick.Forget();
        }

        /// <summary>
        /// Attempts to cancel the current grid placement operation.
        /// </summary>
        public void TryCancel() {
            PointerEventData pointerEvent = new PointerEventData(EventSystem.current);
            pointerEvent.position = Input.mousePosition;
            List<RaycastResult> raycasts = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEvent, raycasts);
            if(raycasts.Exists(r => relatives.Contains(r.gameObject))) {
                return;
            }
            Cancel();
        }

        /// <summary>
        /// Cancels the current grid placement operation.
        /// </summary>
        public void Cancel() {
            InventoryManager.Current.ReviveSlotAt(Center);
            foreach (GridSlotData cell in gridDatas) {
                DestroyImmediate(cell.Skin.gameObject);
            }
            gridDatas.Clear();
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Rotates the grid validator clockwise.
        /// </summary>
        public void Rotate() {
            InventoryManager.Current.ReviveSlotAt(Center);
            foreach (GridSlotData cell in gridDatas) {
                cell.NativeCell = cell.NativeCell.RotateClockwise();
            }
            transform.Rotate(0, 0, -90); // it would be rotate clockwise as `-`
            inventoryManager.CheckPlaceableAt(Center);
        }

        public void SetValid(bool isValid) {
            this.isValid = isValid;            
        }

        /// <summary>
        /// Highlights the grid slots where the validator can be placed.
        /// </summary>
        public void ValidatesAt(Vector2Int gridIndex) {
            foreach (var mask in gridDatas) {
                Vector2Int index = gridRegion.GetIndex(gridIndex, mask.NativeCell);
                int order = gridRegion.GetOrder(gridIndex, mask.NativeCell);
                if (!gridRegion.Contains(index)) {
                    mask.SetSkin(false);
                }
                else if (isValid) { 
                    Validated?.Invoke(this, InventoryManager.Current.Slots[order]);
                }
                else {
                    Invalidated?.Invoke(this, InventoryManager.Current.Slots[order]);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            Rotate();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            isHovering = false;
        }
    }
}