using System;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Toolkid.UIGrid {
    public class InventoryManager : MonoBehaviour, IPageable {
        public static InventoryManager Current { get; set; }
        public GridRegion GridRegion { get => gridRegion; }
        public GridValidator Validator { get => validator; }
        public StackablesInventory Stackables { get => stackables; }

        public ItemSlot[] Slots { get => slots; }
        public int CurrentPage { get => page; set => page = value; }
        public Vector2Int PageSize { get => pageSize; set => pageSize = value; }
        public int PageCount { get => pageSize.x; set => pageSize.x = value; }
        public bool IsLoop { get => false; set => throw new NotImplementedException(); }

        public event EventHandler<int> SlotPageChanged;
        public event EventHandler<ItemSlot> DataChanged;

        [SerializeField] private GridValidator validator;
        [SerializeField, FormerlySerializedAs("gridSystem")] private GridRegion gridRegion;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private StackablesInventory stackables;
        [SerializeField] private PlaceablesDatas placeables;
        [SerializeField] private ItemSlot[] slots;
        [SerializeField] private Dictionary<int, string> placedItems = new Dictionary<int, string>();
        [SerializeField] private Dictionary<int, ItemSlot[]> currentSlots = new Dictionary<int, ItemSlot[]>();

        [SerializeField] private int page = 0;
        [SerializeField] private Vector2Int pageSize = new Vector2Int(3, 1);
        [SerializeField] private bool hasChanged = false;

        [SerializeField] private string operatedItemId = string.Empty;

        private void Start() {
            Current = this;
            GridRegion.Initializes();
            Validator.Initializes(this);
            placeables.Initializes();
            placedItems = new Dictionary<int, string>();
            //m_Stackables.Initialize();        
            for (int initPage = 0; initPage < PageCount; initPage++) {
                ItemSlot[] newSlots = new ItemSlot[GridRegion.GridCount.x * GridRegion.GridCount.y];
                for (int i = 0; i < GridRegion.GridCount.y; i++) {
                    for (int j = 0; j < GridRegion.GridCount.x; j++) {
                        int index = j + GridRegion.GridCount.x * i;
                        if (initPage == 0) {
                            newSlots[index] = new ItemSlot(Instantiate(slotPrefab).GetComponent<RawImage>());
                        }
                        else {
                            newSlots[index] = new ItemSlot(currentSlots[initPage - 1][index].Image);
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

        public ItemSlot GetSlotData(int i) {
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
            if (page >= PageCount) {
                page = PageCount - 1;
            }
            slots = currentSlots[page];
            foreach (var slot in slots) {
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
            ReviveSlotAt(Validator.Center);
            Vector2Int gridIndex = GridRegion.GetIndex(cursorPosition);
            CheckPlaceableAt(gridIndex);
            Validator.transform.position = GridRegion.GetCellCenterWorld(cursorPosition);
            Validator.Center = gridIndex;
        }

        public void ReviveSlotAt(Vector2Int gridIndex) {
            foreach (var mask in Validator.gridDatas) {
                Vector2Int index = GridRegion.GetIndex(gridIndex, mask.NativeCell);
                int order = GridRegion.GetOrder(gridIndex, mask.NativeCell);
                if (GridRegion.Contains(index)) {
                    slots[order].Normalize();
                }
            }
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
                else {
                    if (slots[order].HasUsed) {
                        slots[order].Normalize();
                        isValid = false;
                        mask.SetSkin(true);
                    }
                    else {
                        slots[order].Normalize();
                        mask.SetSkin(true);
                    }
                }
                Validator.SetValid(isValid);
            }
            Validator.ValidatesAt(gridIndex);
        }

        public void StopOperating() {
            operatedItemId = string.Empty;
        }

        public void DoPlaceable(string itemID) {
            operatedItemId = itemID;
            Validator.Preview(PlaceablesDatas.Datas[itemID].Sharp);
        }

        public void PlacesAt(Vector3 position) {
            Vector2Int gridIndex = GridRegion.GetIndex(position);
            PlacesAt(gridIndex);
        }

        public void PlacesAt(Vector2Int index) {
            int center = GridRegion.GetOrder(index, Vector2Int.zero);
            placedItems.Add(center, operatedItemId);
            foreach (var mask in Validator.gridDatas) {
                if (Validator.IsValid) {
                    mask.SetSkin(true);
                    int order = GridRegion.GetOrder(index, mask.NativeCell);
                    slots[order].Build(operatedItemId);
                    DataChanged?.Invoke(this, slots[order]);
                    hasChanged = true;
                }
            }
        }

        public void Places() {
            PlacesAt(Validator.Center);
            Validator.Cancel();
        }
    }
}