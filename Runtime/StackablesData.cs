using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Toolkid.UIGrid {
    [Serializable]
    public class StackablesData : ItemData {        
        [SerializeField] protected int m_Count = 0;
        [SerializeField] protected int m_Max = 0;
        public int Count { get => m_Count; set => m_Count = value; }
        public int Max { get => m_Max; set => m_Max = value; }
    }
}
