﻿using System;
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
    [SerializeField] protected GridRegion gridSystem;
    [SerializeField] protected GameObject slotPrefab;
    [SerializeField] protected Vector2Int pageSize;
    [SerializeField] protected int count;
    [SerializeField] protected int currentPage;
    [SerializeField] protected int pageCount;    
    [SerializeField] protected bool isLoop;

    private Stackables[] stackables;
    private List<SlotData> slots = new List<SlotData>();

    public GridRegion GridSystem { get => gridSystem; }
    public int CurrentPage { get => currentPage; set => currentPage = value; }
    public Vector2Int PageSize { get => pageSize; set => pageSize = value; }
    public int PageCount { get => pageCount; set => pageCount = value; }
    public bool IsLoop { get => isLoop; set => isLoop = value; }


    // Must be Start(), because the size of adaptive canvas initialized on enabled.
    public void Start() {        
        //stackables = GetComponentsInChildren<Stackables>();
        //foreach (Stackables stackable in stackables) {
        //    stackable.Initialize();
        //}
        GridSystem.Initializes(pageSize);
        PageCount = count / (pageSize.x * pageSize.y);
        slots.Clear();
        int id = currentPage * PageCount + 1;
        for (int i = 0; i < GridSystem.GridCount.y; i++) {
            for (int j = 0; j < count; j++) {
                SlotData slot = new SlotData(Instantiate(slotPrefab).GetComponent<RawImage>());
                slots.Add(slot);
                slots[slots.Count - 1].Image.transform.localPosition = Vector3.zero;
                slots[slots.Count - 1].Image.GetComponent<RectTransform>().sizeDelta = GridSystem.Grid.Get2DSize() * 0.95f;
                slots[slots.Count - 1].Image.transform.SetParent(transform);
                slots[slots.Count - 1].Image.transform.position = GridSystem.GetWorldPosition(new Vector2Int(j, i));
                slots[slots.Count - 1].Image.transform.localScale = Vector3.one;                
                slots[slots.Count - 1].Image.GetComponent<Stackables>().Initialize(j); // `j` is order in array, only for testing version.
                slots[slots.Count - 1].Image.GetComponent<Placeables>().Initialize(j);                
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
