using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace Toolkid.UIGrid {

    public interface IItemData {
        int Index { get; }
        string Name { get; }
        string Toolkit { get; }
    }

    [Serializable]
    public class ItemData : IItemData {
        [SerializeField, FormerlySerializedAs("m_ID")] protected int index = -1;
        [SerializeField, FormerlySerializedAs("m_Name")] protected string name = string.Empty;
        [SerializeField, FormerlySerializedAs("m_Tooltip")] protected string tooltip = string.Empty;
        public int Index { get => index; set => index = value; }
        public string Name { get => name; set => name = value; }
        public string Toolkit { get => tooltip; set => tooltip = value; }

        public ItemData(int index = -1, string name = "", string toolkid = "") {
            Index = index;
            Name = name;
            Toolkit = toolkid;
        }

        public void Initialise(int index = -1, string name = "", string toolkid = "") {
            Index = index;
            Name = name;
            Toolkit = toolkid;
        }
    }
}
