using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Toolkid.UIGrid {

    public interface IItemData {
        string Identifier { get; }        
        string Name { get; }
        string Toolkit { get; }
    }

    [Serializable]
    public class ItemData : IItemData {
        public string Identifier { get => identifier; set => identifier = value; }
        public string Name { get => name; set => name = value; }
        public string Toolkit { get => tooltip; set => tooltip = value; }
        public RawImage RawImage { get => rawImage; set => rawImage = value; }

        [SerializeField, FormerlySerializedAs("m_ID"), FormerlySerializedAs("identifier")]
        protected string identifier = string.Empty;
        [SerializeField, FormerlySerializedAs("m_Name")]
        protected string name = string.Empty;
        [SerializeField, FormerlySerializedAs("m_Tooltip")]
        protected string tooltip = string.Empty;
        [SerializeField]
        private RawImage rawImage = null;


        public ItemData(string identifier = "", string name = "", string toolkid = "") {
            Identifier = identifier;
            Name = name;
            Toolkit = toolkid;
        }

        public void Initialise(string identifier = "", string name = "", string toolkid = "") {
            Identifier = identifier;
            Name = name;
            Toolkit = toolkid;
        }
    }
}
