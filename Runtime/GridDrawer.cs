using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Toolkid.GridInventory {
    public class GridDrawer : MonoBehaviour {
        [SerializeField] private int sharpSize = 5;
        [SerializeField] private GameObject m_Prefab;
        public GameObject Prefab { get => m_Prefab; set => m_Prefab = value; }
        public readonly List<GridCell> m_GridCells = new List<GridCell>();

        public void Initialize() {
            GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridSystem.Grid.cellSize;            
        }

        public void Enable(bool[] Sharp) {
            m_GridCells.Clear();
            GetComponent<Grid>().cellSize = InventoryManager.Current.GridSystem.Grid.cellSize;
            for (int i = 0; i < 25; i++) {
                if (Sharp[i]) {
                    m_GridCells.Add(new GridCell(Instantiate(m_Prefab).GetComponent<Image>()));
                    m_GridCells[m_GridCells.Count - 1].skin.transform.localPosition = Vector3.zero;
                    m_GridCells[m_GridCells.Count - 1].skin.GetComponent<RectTransform>().sizeDelta = InventoryManager.Current.GridSystem.Grid.GetSizeInVector2();
                    int midtern = sharpSize * sharpSize / 2;
                    Vector2Int cell = new Vector2Int((i % sharpSize - midtern % sharpSize), -(i / sharpSize - midtern / sharpSize));
                    m_GridCells[m_GridCells.Count - 1].SetCell(cell);
                    m_GridCells[m_GridCells.Count - 1].skin.transform.SetParent(InventoryManager.Current.GridDrawer.transform);
                    m_GridCells[m_GridCells.Count - 1].skin.transform.localPosition = InventoryManager.Current.GridDrawer.GetComponent<Grid>().CellToLocal(new Vector3Int(cell.x, cell.y, 0));
                    m_GridCells[m_GridCells.Count - 1].skin.transform.localScale = Vector3.one;
                }
            }
        }
        public void Disable() {
            foreach (GridCell cell in m_GridCells) {
                DestroyImmediate(cell.skin.gameObject);
            }
            m_GridCells.Clear();
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
    }
}