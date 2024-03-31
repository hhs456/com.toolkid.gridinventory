using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Toolkid.UIGrid {
    public class GridValidator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField, FormerlySerializedAs("m_InventoryManager")] private InventoryManager inventoryManager;
        [SerializeField, FormerlySerializedAs("m_GridSystem")] private GridSystem gridSystem;
        [SerializeField, FormerlySerializedAs("m_SharpSize")] private int sharpSize = 5;
        [SerializeField, FormerlySerializedAs("m_Center")] private Vector2Int center;
        [SerializeField, FormerlySerializedAs("m_Prefab")] private GameObject prefab;
        [SerializeField, FormerlySerializedAs("m_Relatives")] private GameObject[] relatives;
        public GameObject Prefab { get => prefab; set => prefab = value; }
        public Vector2Int Center { get => center; set => center = value; }

        [SerializeField, FormerlySerializedAs("m_GridDatas")] public readonly List<GridSlotData> gridDatas = new List<GridSlotData>();

        bool hasPlaced = false;
        bool isHovering = false;

        public void Initialize() {
            GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridSystem.Grid.cellSize;
        }

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

        public void PlaceOn(Vector2Int index) {
            Center = index;
            foreach (GridSlotData cell in gridDatas) {                
                cell.InventoryIndex = gridSystem.GetIndex(index, cell.NativeCell);
            }
            var anyClick = new GlobalClickDetector<GridValidator>(this, d => !d.isHovering, TryCancel);
            anyClick.Forget();
        }

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
        public void Cancel() {
            foreach (GridSlotData cell in gridDatas) {
                DestroyImmediate(cell.Skin.gameObject);
            }
            gridDatas.Clear();
            transform.localRotation = Quaternion.identity;
        }
        public void Rotate() {
            foreach (GridSlotData cell in gridDatas) {
                cell.NativeCell = cell.NativeCell.RotateClockwise();
            }
            transform.Rotate(0, 0, -90);
            inventoryManager.TryPlaceable(Center);
        }
        public void Placeables() {
            foreach (GridSlotData cell in gridDatas) {
                cell.Skin.color = Color.green;
            }
        }
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