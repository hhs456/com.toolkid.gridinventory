using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Toolkid.UIGrid {
    /// <summary>
    /// Manages the validation and placement of grid-based objects in the Unity environment.
    /// </summary>
    public class GridValidator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private GridRegion gridSystem;
        [SerializeField] private int sharpSize = 5;
        [SerializeField] private Vector2Int center;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject[] relatives;
        public GameObject Prefab { get => prefab; set => prefab = value; }
        public Vector2Int Center { get => center; set => center = value; }

        [SerializeField] public readonly List<GridSlotData> gridDatas = new List<GridSlotData>();
                
        private bool isHovering = false;

        /// <summary>
        /// Initializes the grid validator component.
        /// </summary>
        public void Initializes() {
            GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridSystem.Grid.cellSize;
        }

        /// <summary>
        /// Generates a preview of the grid based on the provided sharp array.
        /// </summary>
        /// <param name="Sharp">An array indicating the shape of the grid.</param>
        public void Preview(bool[] Sharp) {
            gridDatas.Clear();
            GetComponent<Grid>().cellSize = InventoryManager.Current.GridSystem.Grid.cellSize;
            for (int i = 0; i < 25; i++) {
                if (Sharp[i]) {
                    gridDatas.Add(new GridSlotData(Instantiate(prefab).GetComponent<RawImage>()));
                    int lastIndex = gridDatas.Count - 1;
                    gridDatas[lastIndex].Skin.transform.localPosition = Vector3.zero;
                    gridDatas[lastIndex].Skin.GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridSystem.Grid.Get2DSize();
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
                cell.InventoryIndex = gridSystem.GetIndex(index, cell.NativeCell);
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
            foreach (GridSlotData cell in gridDatas) {
                cell.NativeCell = cell.NativeCell.RotateClockwise();
            }
            transform.Rotate(0, 0, -90);
            inventoryManager.TryPlaceable(Center);
        }

        /// <summary>
        /// Highlights the grid slots where the validator can be placed.
        /// </summary>
        public void Placeables() {
            foreach (GridSlotData cell in gridDatas) {
                cell.Skin.color = Color.green;
            }
        }

        /// <summary>
        /// Invalidates the grid slots where the validator cannot be placed.
        /// </summary>
        public void Invalidate() {
            foreach (GridSlotData cell in gridDatas) {
                cell.Skin.color = Color.red;
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