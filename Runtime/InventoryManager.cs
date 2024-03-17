using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Toolkid.UIGrid;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Current { get; set; }
    public GridSystem GridSystem { get => m_GridSystem; }
    public GridValidator GridDrawer { get => m_GridDrawer; }
    public StackablesManager Stackables { get => m_Stackables; }
    public bool IsPlaceable { get => m_IsPlaceable; }

    [SerializeField] private GridValidator m_GridDrawer;
    [SerializeField] private GridSystem m_GridSystem;
    [SerializeField] private GameObject m_SlotPrefab;
    [SerializeField] private StackablesManager m_Stackables;
    [SerializeField] private PlaceablesDatas m_Placeables;
    [SerializeField] SlotData[] slots;

    [SerializeField] bool m_IsPlaceable = false;

    void Start() {
        Current = this;
        GridSystem.Initialize();
        GridDrawer.Initialize();
        m_Placeables.Initialize();
        //m_Stackables.Initialize();
        slots = new SlotData[GridSystem.GridCount.x * GridSystem.GridCount.y];
        for (int i = 0; i < GridSystem.GridCount.y; i++) {            
            for (int j = 0; j < GridSystem.GridCount.x; j++) {
                int index = j + GridSystem.GridCount.x * i;
                slots[index] = new SlotData(Instantiate(m_SlotPrefab).GetComponent<RawImage>());                
                slots[index].Image.transform.localPosition = Vector3.zero;
                slots[index].Image.GetComponent<RectTransform>().sizeDelta = GridSystem.Grid.Get2DSize() * 0.95f;
                slots[index].Image.transform.SetParent(transform);
                slots[index].Image.transform.position = GridSystem.GetWorldPosition(new Vector2Int(j, i));
                slots[index].Image.transform.localScale = Vector3.one;
            }
        }
    }


    public void OnHover(Vector3 position) {
        Vector2Int gridIndex = GridSystem.GetIndex(position);
        TryPlaceable(gridIndex);
        GridDrawer.transform.position = GridSystem.GetCellCenterWorld(position);
    }

    public bool TryPlaceable(Vector2Int gridIndex) {
        bool isPlaceable = true;
        foreach (var mask in GridDrawer.m_GridDatas) {
            mask.SetSkin(true);
            Vector2Int index = GridSystem.GetIndex(gridIndex, mask.NativeCell);
            int order = GridSystem.GetOrder(gridIndex, mask.NativeCell);
            if (GridSystem.TryArea(index)) {
                isPlaceable = false;
                GridDrawer.Invalidate();
                mask.SetSkin(false);
            }
        }
        if (isPlaceable) {
            foreach (var mask in GridDrawer.m_GridDatas) {
                mask.SetSkin(true);
                Vector2Int index = GridSystem.GetIndex(gridIndex, mask.NativeCell);
                int order = GridSystem.GetOrder(gridIndex, mask.NativeCell);
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
        return m_IsPlaceable;
    }

    public void OnPlaceable(bool[] Sharp) {
        GridDrawer.Preview(Sharp);
    }
    public void OnPlace(Vector3 position) {
        Vector2Int gridIndex = GridSystem.GetIndex(position);
        int center = GridSystem.GetOrder(gridIndex, Vector2Int.zero);
        foreach (var mask in GridDrawer.m_GridDatas) {
            if (IsPlaceable) {
                mask.SetSkin(true);
                int order = GridSystem.GetOrder(gridIndex, mask.NativeCell);
                slots[order].SetData(Color.gray);
                slots[order].SetData(center);
            }
        }
    }
    public void OnPlace(Vector2Int index) {        
        int center = GridSystem.GetOrder(index, Vector2Int.zero);
        foreach (var mask in GridDrawer.m_GridDatas) {
            if (IsPlaceable) {
                mask.SetSkin(true);
                int order = GridSystem.GetOrder(index, mask.NativeCell);
                slots[order].SetData(Color.gray);
                slots[order].SetData(center);
            }
        }
    }
    public void Places() {
        OnPlace(GridDrawer.Center);
        GridDrawer.Cancel();
    }
}
