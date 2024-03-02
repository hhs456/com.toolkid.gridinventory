using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceMask : MonoBehaviour
{
    [SerializeField] private int sharpSize = 5;
    [SerializeField] private GameObject m_Prefab;
    public GameObject Prefab { get => m_Prefab; set => m_Prefab = value; }
    public readonly List<MaskGrid> m_MaskGrids = new List<MaskGrid>();

    public void Enable(bool[] Sharp) {        
        m_MaskGrids.Clear();
        GridSystem.Current.Mask.GetComponent<Grid>().cellSize = GridSystem.Current.Grid.cellSize;
        for (int i = 0; i < 25; i++) {
            if (Sharp[i]) {
                m_MaskGrids.Add(new MaskGrid(Instantiate(m_Prefab).GetComponent<Image>()));
                m_MaskGrids[m_MaskGrids.Count - 1].skin.transform.localPosition = Vector3.zero;
                m_MaskGrids[m_MaskGrids.Count - 1].skin.GetComponent<RectTransform>().sizeDelta = GridSystem.Current.GetSizeIn2DFrom(GridSystem.Current.Grid);
                int midtern = sharpSize * sharpSize / 2;
                Vector2Int cell = new Vector2Int((i % sharpSize - midtern % sharpSize) , -(i / sharpSize - midtern / sharpSize));
                m_MaskGrids[m_MaskGrids.Count - 1].SetCell(cell);
                m_MaskGrids[m_MaskGrids.Count - 1].skin.transform.SetParent(GridSystem.Current.Mask.transform);
                m_MaskGrids[m_MaskGrids.Count - 1].skin.transform.localPosition = GridSystem.Current.Mask.GetComponent<Grid>().CellToLocal(new Vector3Int(cell.x, cell.y, 0));
                m_MaskGrids[m_MaskGrids.Count - 1].skin.transform.localScale = Vector3.one;
            }
        }
    }
    public void Disable() {        
        foreach (MaskGrid grid in m_MaskGrids) {
            DestroyImmediate(grid.skin.gameObject);
        }
        m_MaskGrids.Clear();        
    }
    public void Placeables() {
        foreach (MaskGrid grid in m_MaskGrids) {
            grid.skin.color = Color.green;
        }
    }
    public void Invalidate() {
        foreach (MaskGrid grid in m_MaskGrids) {
            grid.skin.color = Color.red;
        }
    }
}
public class MaskGrid {
    public Image skin;
    public Vector2Int cell;

    public MaskGrid(Image skin) {
        this.skin = skin;
    }
    public void SetCell(Vector2Int cell) {
        this.cell = cell;
    }
    public void SetSkin(bool enable) {
        skin.enabled = enable;
    }
}
