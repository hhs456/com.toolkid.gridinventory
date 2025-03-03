using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Toolkid.UIGrid;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StackablesInventory : MonoBehaviour, IPageable
{

    public GridRegion GridSystem { get => gridRegion; }
    public int CurrentPage { get => currentPage; set => currentPage = value; }
    public Vector2Int PageSize { get => pageSize; set => pageSize = value; }
    public int PageCount { get => pageCount; set => pageCount = value; }
    public bool IsLoop { get => isLoop; set => isLoop = value; }

    [SerializeField, FormerlySerializedAs("gridSystem")] protected GridRegion gridRegion;
    [SerializeField] protected GameObject slotPrefab;
    [SerializeField] protected Vector2Int pageSize;
    [SerializeField] protected int count;
    [SerializeField] protected int currentPage;
    [SerializeField] protected int pageCount;    
    [SerializeField] protected bool isLoop;
    
    private List<ItemSlot> slots = new List<ItemSlot>();


    // Must be Start(), because the size of adaptive canvas initialized on enabled.
    public void Start() {
        //stackables = GetComponentsInChildren<Stackables>();
        //foreach (Stackables stackable in stackables) {
        //    stackable.Initialize();
        //}
        GridSystem.Initializes(pageSize);
        PageCount = count / (pageSize.x * pageSize.y);
        slots.Clear();
        string[] ids = new string[] { "Generalitem_Common_Agriculture_Normal_Cashseed_002", "Generalitem_Common_Agriculture_Normal_Cashseed_001", "Generalitem_Common_Agriculture_Normal_Cashseed_005", "Generalitem_Common_Agriculture_Normal_Cashseed_007" };
        for (int i = 0; i < GridSystem.GridCount.y; i++) {
            for (int j = 0; j < count; j++) {
                ItemSlot slot = new ItemSlot(Instantiate(slotPrefab).GetComponent<RawImage>());
                slots.Add(slot);
                slots[slots.Count - 1].Image.transform.localPosition = Vector3.zero;
                slots[slots.Count - 1].Image.GetComponent<RectTransform>().sizeDelta = GridSystem.Grid.Get2DSize() * 0.95f;
                slots[slots.Count - 1].Image.transform.SetParent(transform);
                slots[slots.Count - 1].Image.transform.position = GridSystem.GetWorldPosition(new Vector2Int(j, i));
                slots[slots.Count - 1].Image.transform.localScale = Vector3.one;
                slots[slots.Count - 1].Image.transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<Texture>(ids[j]);
                slots[slots.Count - 1].Image.GetComponent<Placeables>().Initialize(ids[j]);
            }
        }      
    }

    /// <summary>
    /// Goes to the next page in the inventory.
    /// </summary>
    public void NextPage() {
        currentPage++;
        if (currentPage > pageCount) {
            currentPage--;
            if (isLoop) {
                foreach (var slot in slots) {
                    var rect = slot.Image.GetComponent<RectTransform>();
                    rect.localPosition += new Vector3(GridSystem.Grid.cellSize.x * pageSize.x * currentPage, 0, 0);
                }
                currentPage = 0;
            }
        }
        else {
            foreach (var slot in slots) {
                var rect = slot.Image.GetComponent<RectTransform>();
                rect.localPosition += new Vector3(-GridSystem.Grid.cellSize.x * pageSize.x, 0, 0);
            }
        }
    }

    /// <summary>
    /// Goes to the previous page in the inventory.
    /// </summary>
    public void PrevPage() {
        currentPage--;
        if (currentPage < 0) {
            currentPage++;
            if (isLoop) {
                foreach (var slot in slots) {
                    var rect = slot.Image.GetComponent<RectTransform>();
                    rect.localPosition += new Vector3(-GridSystem.Grid.cellSize.x * pageSize.x * pageCount, 0, 0);
                }
                currentPage = pageCount;
            }
        }
        else {
            foreach (var slot in slots) {
                var rect = slot.Image.GetComponent<RectTransform>();
                rect.localPosition += new Vector3(GridSystem.Grid.cellSize.x * pageSize.x, 0, 0);
            }
        }
    }
}
