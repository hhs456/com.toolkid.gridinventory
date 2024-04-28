using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace Toolkid.UIGrid {
    [Serializable]
    public abstract class ItemData
    {
        [SerializeField, FormerlySerializedAs("m_ID")] protected int index = -1;
        [SerializeField, FormerlySerializedAs("m_Name")] protected string name = string.Empty;
        [SerializeField, FormerlySerializedAs("m_Tooltip")] protected string tooltip = string.Empty;
        public int Index { get => index; set => index = value; }
        public string Name { get => name; set => name = value; }
        public string Toolkit { get => tooltip; set => tooltip = value; }
    }
}
