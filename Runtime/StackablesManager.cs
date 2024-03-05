using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Toolkid.GridInventory;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class StackablesManager : MonoBehaviour
{
    Stackables[] stackables;

    public void Initialize() {
        stackables = GetComponentsInChildren<Stackables>();
        foreach (Stackables stackable in stackables) {
            stackable.Initialize();
        }
    }
}
