using System;
using UnityEngine;
namespace Toolkid.UIGrid {
    [Serializable]
    public abstract class ItemData
    {
        [SerializeField] protected int m_ID = -1;
        [SerializeField] protected string m_Name = string.Empty;
        [SerializeField] protected string m_Tooltip = string.Empty;
        public int ID { get => m_ID; set => m_ID = value; }
        public string Name { get => m_Name; set => m_Name = value; }
        public string Toolkit { get => m_Tooltip; set => m_Tooltip = value; }
    }
}
