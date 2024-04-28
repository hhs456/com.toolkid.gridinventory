using System;
using System.Collections;
using System.Collections.Generic;
using Toolkid.UIGrid;
using UnityEngine;
using UnityEngine.Serialization;
[Serializable]
public class PlaceablesData : ItemData {
    const int length = 5;
    [SerializeField, FormerlySerializedAs("enabled")] private bool[] sharp = new bool[length * length];
    public bool[] Sharp { get => sharp; set => sharp = value; }

    public PlaceablesData() {
        sharp = new bool[length * length];
    }
}