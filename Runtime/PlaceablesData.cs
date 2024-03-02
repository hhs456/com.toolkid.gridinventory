using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlaceablesData {
    const int length = 5;
    [SerializeField] private int m_ID = -1;
    [SerializeField] private string m_Name = string.Empty;
    [SerializeField] private bool[] m_Sharp = new bool[length * length];
    public int ID { get => m_ID; set => m_ID = value; }
    public string Name { get => m_Name; set => m_Name = value; }
    public bool[] Sharp { get => m_Sharp; set => m_Sharp = value; }
}