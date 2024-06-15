using System;
using System.Collections.Generic;
using Toolkid.UIGrid;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    [SerializeField] private GridValidator validator;
    [SerializeField, FormerlySerializedAs("gridSystem")] private GridRegion gridRegion;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private StackablesInventory stackables;
    [SerializeField] private PlaceablesDatas placeables;
    [SerializeField] private SlotData[] slots;
    [SerializeField] private Dictionary<int, SlotData[]> currentSlots = new Dictionary<int, SlotData[]>();

    [SerializeField] private int page = 0;
    [SerializeField] private int pageSize = 3;
    [SerializeField] private bool hasChanged = false;

    [SerializeField] private int operatedItemId = -1;

    public static InventoryManager Current { get; set; }
    public GridRegion GridRegion { get => gridRegion; }
    public GridValidator Validator { get => validator; }
    public StackablesInventory Stackables { get => stackables; }

    public SlotData[] Slots { get => slots; }

    public event EventHandler<int> SlotDataChanged;
    public event EventHandler<int> SlotPageChanged;

    private void Start() {
        Current = this;
        GridRegion.Initializes();
        Validator.Initializes();
        placeables.Initializes();
        //m_Stackables.Initialize();        
        for (int initPage = 0; initPage < pageSize; initPage++) {
            SlotData[] newSlots = new SlotData[GridRegion.GridCount.x * GridRegion.GridCount.y];
            for (int i = 0; i < GridRegion.GridCount.y; i++) {
                for (int j = 0; j < GridRegion.GridCount.x; j++) {
                    int index = j + GridRegion.GridCount.x * i;
                    if (initPage == 0) {
                        newSlots[index] = new SlotData(Instantiate(slotPrefab).GetComponent<RawImage>());
                    }
                    else {
                        newSlots[index] = new SlotData(currentSlots[initPage - 1][index].Image);
                    }
                    newSlots[index].Image.transform.localPosition = Vector3.zero;
                    newSlots[index].Image.GetComponent<RectTransform>().sizeDelta = GridRegion.Grid.Get2DSize() * 0.95f;
                    newSlots[index].Image.transform.SetParent(transform);
                    newSlots[index].Image.transform.position = GridRegion.GetWorldPosition(new Vector2Int(j, i));
                    newSlots[index].Image.transform.localScale = Vector3.one;
                }
            }
            currentSlots.Add(initPage, newSlots);
        }
        slots = currentSlots[0];
    }

    public SlotData GetSlotData(int i) {
        return slots[i];
    }

    public void TrySaveSlots() {
        if (hasChanged) {
            currentSlots[page] = slots;
            hasChanged = false;
        }
    }

    public void NextPage() {
        TrySaveSlots();
        page++;
        if(page >= pageSize) {
            page = pageSize - 1;
        }
        slots = currentSlots[page];
        foreach(var slot in slots) {
            slot.Normalize();
        }
        SlotPageChanged?.Invoke(this, page);
    }

    public void PrevPage() {
        TrySaveSlots();
        page--;
        if (page < 0) {
            page = 0;
        }
        slots = currentSlots[page];
        foreach (var slot in slots) {
            slot.Normalize();
        }
        SlotPageChanged?.Invoke(this, page);
    }

    /// <summary>
    /// Called when the mouse hovers over a position in the grid.
    /// </summary>
    /// <param name="cursorPosition">The position in world space.</param>
    public void OnDragging(Vector3 cursorPosition) {
        Vector2Int gridIndex = GridRegion.GetIndex(cursorPosition);
        CheckPlaceableAt(gridIndex);
        Validator.transform.position = GridRegion.GetCellCenterWorld(cursorPosition);
    }

    /// <summary>
    /// Tries to determine if the specified grid index is placeable.
    /// </summary>
    /// <param name="gridIndex">The grid index to check.</param>
    /// <returns>True if the grid index is placeable, false otherwise.</returns>
    public void CheckPlaceableAt(Vector2Int gridIndex) {
        bool isValid = true;        
        foreach (var mask in Validator.gridDatas) {
            Vector2Int index = GridRegion.GetIndex(gridIndex, mask.NativeCell);
            int order = GridRegion.GetOrder(gridIndex, mask.NativeCell);
            if (!GridRegion.Contains(index)) {
                isValid = false;
                mask.SetSkin(false);
            }
            else if (slots[order].HasUsed) {
                isValid = false;                
                mask.SetSkin(true);
            }
            else {
                mask.SetSkin(true);
            }
            if (isValid) {
                Validator.Validates();
            }
            else {
                Validator.Invalidates();
            }
        }
    }

    public void StopOperating() {
        operatedItemId = -1;
    }

    public void DoPlaceable(int objectID) {
        operatedItemId = objectID;
        Validator.Preview(PlaceablesDatas.Datas[objectID].Sharp);
    }

    public void PlacesAt(Vector3 position) {
        Vector2Int gridIndex = GridRegion.GetIndex(position);
        PlacesAt(gridIndex);
    }

    public void PlacesAt(Vector2Int index) {
        int center = GridRegion.GetOrder(index, Vector2Int.zero);
        foreach (var mask in Validator.gridDatas) {
            if (Validator.IsValid) {
                mask.SetSkin(true);
                int order = GridRegion.GetOrder(index, mask.NativeCell);
                slots[order].Build(center, operatedItemId);
                SlotDataChanged?.Invoke(this, order);
                hasChanged = true;
            }
        }
    }

    public void Places() {
        PlacesAt(Validator.Center);
        Validator.Cancel();
    }
}
