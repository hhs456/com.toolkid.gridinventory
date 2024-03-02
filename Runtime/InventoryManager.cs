using System.Collections;
using System.Collections.Generic;
using Toolkid.GridInventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Current { get; set; }
    public GridSystem GridSystem { get => m_GridSystem; }
    public GridDrawer GridDrawer { get => m_GridDrawer; }

    [SerializeField] private GridDrawer m_GridDrawer;
    [SerializeField] private GridSystem m_GridSystem;
    [SerializeField] private GameObject m_SlotPrefab;
    List<Image> slots = new List<Image>();

    void OnEnable() {
        Current = this;
        GridSystem.Initialize();
        GridDrawer.Initialize();
        slots.Clear();
        for (int i = 0; i < GridSystem.GridCount.x; i++) {
            for (int j = 0; j < GridSystem.GridCount.y; j++) {
                slots.Add(Instantiate(m_SlotPrefab).GetComponent<Image>());
                slots[slots.Count - 1].transform.localPosition = Vector3.zero;
                slots[slots.Count - 1].GetComponent<RectTransform>().sizeDelta = GridSystem.Grid.GetSizeInVector2() * 0.95f;
                slots[slots.Count - 1].transform.SetParent(transform);
                slots[slots.Count - 1].transform.position = GridSystem.GetWorldPosition(new Vector2Int(i, j));
                slots[slots.Count - 1].transform.localScale = Vector3.one;
            }
        }
    }


    public void OnHover(Vector3 placePos) {
        Vector2Int gridIndex = GridSystem.GetIndex(placePos);
        bool isPlaceable = true;
        foreach (var mask in GridDrawer.m_GridCells) {
            mask.SetSkin(true);
            if (GridSystem.TryArea(gridIndex, mask.position)) {
                isPlaceable = false;
                GridDrawer.Invalidate();
                mask.SetSkin(false);
            }
        }
        if (isPlaceable) {
            GridDrawer.Placeables();
        }

        GridDrawer.transform.position = GridSystem.GetCellCenterWorld(placePos);
    }

    public void OnPlaceable(bool[] Sharp) {
        GridDrawer.Enable(Sharp);
    }
    public void OnPlace() {
        GridDrawer.Disable();
    }

}
