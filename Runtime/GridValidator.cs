using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Toolkid.UIGrid {
    public class GridValidator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private InventoryManager m_InventoryManager;
        [SerializeField] private GridSystem m_GridSystem;
        [SerializeField] private int m_SharpSize = 5;
        [SerializeField] private Vector2Int m_Center;
        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private GameObject[] m_Relatives;
        public GameObject Prefab { get => m_Prefab; set => m_Prefab = value; }
        public Vector2Int Center { get => m_Center; set => m_Center = value; }

        public readonly List<GridSlotData> m_GridDatas = new List<GridSlotData>();

        bool hasPlaced = false;
        bool isHovering = false;

        public void Initialize() {
            GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridSystem.Grid.cellSize;
        }

        public void Preview(bool[] Sharp) {
            m_GridDatas.Clear();
            GetComponent<Grid>().cellSize = InventoryManager.Current.GridSystem.Grid.cellSize;
            for (int i = 0; i < 25; i++) {
                if (Sharp[i]) {
                    m_GridDatas.Add(new GridSlotData(Instantiate(m_Prefab).GetComponent<RawImage>()));
                    int lastIndex = m_GridDatas.Count - 1;
                    m_GridDatas[lastIndex].Skin.transform.localPosition = Vector3.zero;
                    m_GridDatas[lastIndex].Skin.GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridSystem.Grid.Get2DSize();
                    int midtern = m_SharpSize * m_SharpSize / 2;
                    Vector2Int cell = new Vector2Int((i % m_SharpSize - midtern % m_SharpSize), -(i / m_SharpSize - midtern / m_SharpSize));
                    m_GridDatas[lastIndex].SetCell(cell);
                    m_GridDatas[lastIndex].Skin.transform.SetParent(InventoryManager.Current.GridDrawer.transform);
                    m_GridDatas[lastIndex].Skin.transform.localPosition = InventoryManager.Current.GridDrawer.GetComponent<Grid>().CellToLocal(new Vector3Int(cell.x, cell.y, 0));
                    m_GridDatas[lastIndex].Skin.transform.localScale = Vector3.one;
                }
            }
        }

        public void PlaceOn(Vector2Int index) {
            Center = index;
            foreach (GridSlotData cell in m_GridDatas) {                
                cell.InventoryIndex = m_GridSystem.GetIndex(index, cell.NativeCell);
            }
            var anyClick = new GlobalClickDetector<GridValidator>(this, d => !d.isHovering, TryCancel);
            anyClick.Forget();
        }

        public void TryCancel() {
            PointerEventData pointerEvent = new PointerEventData(EventSystem.current);
            pointerEvent.position = Input.mousePosition;
            List<RaycastResult> raycasts = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEvent, raycasts);
            if(raycasts.Exists(r => m_Relatives.Contains(r.gameObject))) {
                return;
            }
            Cancel();
        }
        public void Cancel() {
            foreach (GridSlotData cell in m_GridDatas) {
                DestroyImmediate(cell.Skin.gameObject);
            }
            m_GridDatas.Clear();
            transform.localRotation = Quaternion.identity;
        }
        public void Rotate() {
            foreach (GridSlotData cell in m_GridDatas) {
                cell.NativeCell = cell.NativeCell.RotateClockwise();
            }
            transform.Rotate(0, 0, -90);
            m_InventoryManager.TryPlaceable(Center);
        }
        public void Placeables() {
            foreach (GridSlotData cell in m_GridDatas) {
                cell.Skin.color = Color.green;
            }
        }
        public void Invalidate() {
            foreach (GridSlotData cell in m_GridDatas) {
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