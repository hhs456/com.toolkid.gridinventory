using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Toolkid.GridInventory {
    public class GridValidator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private InventoryManager m_InventoryManager;
        [SerializeField] private GridSystem m_GridSystem;
        [SerializeField] private int m_SharpSize = 5;
        [SerializeField] private Vector2Int m_Center;
        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private GameObject[] m_Relatives;
        public GameObject Prefab { get => m_Prefab; set => m_Prefab = value; }
        public Vector2Int Center { get => m_Center; set => m_Center = value; }

        public readonly List<GridCell> m_GridCells = new List<GridCell>();

        bool hasPlaced = false;
        bool isHovering = false;

        public void Initialize() {
            GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridSystem.Grid.cellSize;
        }

        public void Enable(bool[] Sharp) {
            m_GridCells.Clear();
            GetComponent<Grid>().cellSize = InventoryManager.Current.GridSystem.Grid.cellSize;
            for (int i = 0; i < 25; i++) {
                if (Sharp[i]) {
                    m_GridCells.Add(new GridCell(Instantiate(m_Prefab).GetComponent<RawImage>()));
                    m_GridCells[m_GridCells.Count - 1].skin.transform.localPosition = Vector3.zero;
                    m_GridCells[m_GridCells.Count - 1].skin.GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridSystem.Grid.Get2DSize();
                    int midtern = m_SharpSize * m_SharpSize / 2;
                    Vector2Int cell = new Vector2Int((i % m_SharpSize - midtern % m_SharpSize), -(i / m_SharpSize - midtern / m_SharpSize));
                    m_GridCells[m_GridCells.Count - 1].SetCell(cell);
                    m_GridCells[m_GridCells.Count - 1].skin.transform.SetParent(InventoryManager.Current.GridDrawer.transform);
                    m_GridCells[m_GridCells.Count - 1].skin.transform.localPosition = InventoryManager.Current.GridDrawer.GetComponent<Grid>().CellToLocal(new Vector3Int(cell.x, cell.y, 0));
                    m_GridCells[m_GridCells.Count - 1].skin.transform.localScale = Vector3.one;
                }
            }
        }
        public void Placing(Vector2Int index) {
            Center = index;
            foreach (GridCell cell in m_GridCells) {                
                cell.inventoryIndex = m_GridSystem.GetIndex(index, cell.nativeCell);
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
            foreach (GridCell cell in m_GridCells) {
                DestroyImmediate(cell.skin.gameObject);
            }
            m_GridCells.Clear();
            transform.localRotation = Quaternion.identity;
        }
        public void Rotate() {
            foreach (GridCell cell in m_GridCells) {
                cell.nativeCell = cell.nativeCell.RotateClockwise();
            }
            transform.Rotate(0, 0, -90);
            m_InventoryManager.TryPlaceable(Center);
        }
        public void Placeables() {
            foreach (GridCell cell in m_GridCells) {
                cell.skin.color = Color.green;
            }
        }
        public void Invalidate() {
            foreach (GridCell cell in m_GridCells) {
                cell.skin.color = Color.red;
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