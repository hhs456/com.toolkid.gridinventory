using System;
using System.Collections;
using System.Collections.Generic;
using Toolkid.GridInventory;
using UnityEngine;
[Serializable]
public class PlaceablesData : ItemData {
    const int length = 5;
    [SerializeField] private bool[] m_Sharp = new bool[length * length];
    public bool[] Sharp { get => m_Sharp; set => m_Sharp = value; }

    public PlaceablesData() {
        m_Sharp = new bool[length * length];
    }
}