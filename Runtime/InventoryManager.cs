using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Toolkid.GridInventory;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Current { get; set; }
    public GridSystem GridSystem { get => m_GridSystem; }
    public GridDrawer GridDrawer { get => m_GridDrawer; }
    public StackablesManager Stackables { get => m_Stackables; }
    public bool IsPlaceable { get => m_IsPlaceable; }

    [SerializeField] private GridDrawer m_GridDrawer;
    [SerializeField] private GridSystem m_GridSystem;
    [SerializeField] private GameObject m_SlotPrefab;
    [SerializeField] private StackablesManager m_Stackables;
    [SerializeField] private PlaceablesDatas m_Placeables;
    List<SlotData> slots = new List<SlotData>();

    [SerializeField] bool m_IsPlaceable = false;

    public Dictionary<int, PlaceablesData> Placeables { get => m_Placeables.Datas; }

    void OnEnable() {
        Current = this;
        GridSystem.Initialize();
        GridDrawer.Initialize();
        m_Placeables.Initialize();
        //m_Stackables.Initialize();
        slots.Clear();
        for (int i = 0; i < GridSystem.GridCount.y; i++) {            
            for (int j = 0; j < GridSystem.GridCount.x; j++) {
                SlotData slot = new SlotData(Instantiate(m_SlotPrefab).GetComponent<Image>());
                slots.Add(slot);
                slots[slots.Count - 1].Image.transform.localPosition = Vector3.zero;
                slots[slots.Count - 1].Image.GetComponent<RectTransform>().sizeDelta = GridSystem.Grid.GetSizeInVector2() * 0.95f;
                slots[slots.Count - 1].Image.transform.SetParent(transform);
                slots[slots.Count - 1].Image.transform.position = GridSystem.GetWorldPosition(new Vector2Int(j, i));
                slots[slots.Count - 1].Image.transform.localScale = Vector3.one;
            }
        }
    }


    public void OnHover(Vector3 position) {
        Vector2Int gridIndex = GridSystem.GetIndex(position);
        bool isPlaceable = true;
        foreach (var mask in GridDrawer.m_GridCells) {
            mask.SetSkin(true);
            Vector2Int index = GridSystem.GetIndex(gridIndex, mask.position);
            int order = GridSystem.GetOrder(gridIndex, mask.position);
            if (GridSystem.TryArea(index)) {
                isPlaceable = false;
                GridDrawer.Invalidate();
                mask.SetSkin(false);
            }
        }
        if (isPlaceable) {
            foreach (var mask in GridDrawer.m_GridCells) {
                mask.SetSkin(true);
                Vector2Int index = GridSystem.GetIndex(gridIndex, mask.position);
                int order = GridSystem.GetOrder(gridIndex, mask.position);
                if (slots[order].HasUsed) {
                    isPlaceable = false;
                    GridDrawer.Invalidate();                    
                }
            }
            if (isPlaceable) {                
                GridDrawer.Placeables();
            }
        }
        m_IsPlaceable = isPlaceable;
        GridDrawer.transform.position = GridSystem.GetCellCenterWorld(position);
    }

    public void OnPlaceable(bool[] Sharp) {
        GridDrawer.Enable(Sharp);
    }
    public void OnPlace(Vector3 position) {
        Vector2Int gridIndex = GridSystem.GetIndex(position);
        int center = GridSystem.GetOrder(gridIndex, Vector2Int.zero);
        foreach (var mask in GridDrawer.m_GridCells) {
            if (IsPlaceable) {
                mask.SetSkin(true);
                int order = GridSystem.GetOrder(gridIndex, mask.position);
                slots[order].SetData(Color.gray);
                slots[order].SetData(center);
            }
        }
        GridDrawer.Disable();
    }
}
