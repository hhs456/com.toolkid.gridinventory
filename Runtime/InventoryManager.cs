using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Toolkid.UIGrid;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Current { get; set; }
    public GridRegion GridSystem { get => gridSystem; }
    public GridValidator Validator { get => validator; }
    public StackablesInventory Stackables { get => stackables; }
    public bool IsPlaceable { get => isPlaceable; }

    [SerializeField, FormerlySerializedAs("gridDrawer")] private GridValidator validator;
    [SerializeField, FormerlySerializedAs("m_GridSystem")] private GridRegion gridSystem;
    [SerializeField, FormerlySerializedAs("m_SlotPrefab")] private GameObject slotPrefab;
    [SerializeField, FormerlySerializedAs("m_Stackables")] private StackablesInventory stackables;
    [SerializeField, FormerlySerializedAs("m_Placeables")] private PlaceablesDatas placeables;
    [SerializeField] SlotData[] slots;

    [SerializeField] bool isPlaceable = false;

    void Start() {
        Current = this;
        GridSystem.Initializes();
        Validator.Initialize();
        placeables.Initialize();
        //m_Stackables.Initialize();
        slots = new SlotData[GridSystem.GridCount.x * GridSystem.GridCount.y];
        for (int i = 0; i < GridSystem.GridCount.y; i++) {            
            for (int j = 0; j < GridSystem.GridCount.x; j++) {
                int index = j + GridSystem.GridCount.x * i;
                slots[index] = new SlotData(Instantiate(slotPrefab).GetComponent<RawImage>());                
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
        Validator.transform.position = GridSystem.GetCellCenterWorld(position);
    }

    public bool TryPlaceable(Vector2Int gridIndex) {
        bool isPlaceable = true;
        foreach (var mask in Validator.gridDatas) {            
            Vector2Int index = GridSystem.GetIndex(gridIndex, mask.NativeCell);
            int order = GridSystem.GetOrder(gridIndex, mask.NativeCell);
            if (!GridSystem.Contains(index)) {
                isPlaceable = false;
                Validator.Invalidate();
                mask.SetSkin(false);                
            }
            else if (slots[order].HasUsed) {
                isPlaceable = false;
                Validator.Invalidate();
                mask.SetSkin(true);
            }
            else {
                mask.SetSkin(true);
            }
        }
        this.isPlaceable = isPlaceable;
        if (isPlaceable) {
            Validator.Placeables();
        }
        return this.isPlaceable;
    }

    public void OnPlaceable(bool[] Sharp) {
        Validator.Preview(Sharp);
    }
    public void OnPlace(Vector3 position) {
        Vector2Int gridIndex = GridSystem.GetIndex(position);
        OnPlace(gridIndex);
    }
    public void OnPlace(Vector2Int index) {        
        int center = GridSystem.GetOrder(index, Vector2Int.zero);
        foreach (var mask in Validator.gridDatas) {
            if (IsPlaceable) {
                mask.SetSkin(true);
                int order = GridSystem.GetOrder(index, mask.NativeCell);
                slots[order].SetData(Color.gray);
                slots[order].SetData(center);
            }
        }
    }
    public void Places() {
        OnPlace(Validator.Center);
        Validator.Cancel();
    }
}
