﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Toolkid.UIGrid;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class StackablesManager : MonoBehaviour
{
    public GridSystem GridSystem { get => m_GridSystem; }
    [SerializeField] protected GridSystem m_GridSystem;
    [SerializeField] protected GameObject m_SlotPrefab;
    Stackables[] stackables;
    List<SlotData> slots = new List<SlotData>();

    // Must be Start(), because the size of adaptive canvas initialized on enabled.
    public void Start() {        
        //stackables = GetComponentsInChildren<Stackables>();
        //foreach (Stackables stackable in stackables) {
        //    stackable.Initialize();
        //}
        GridSystem.Initialize();
        slots.Clear();
        for (int i = 0; i < GridSystem.GridCount.y; i++) {
            for (int j = 0; j < GridSystem.GridCount.x; j++) {
                SlotData slot = new SlotData(Instantiate(m_SlotPrefab).GetComponent<RawImage>());
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
}
