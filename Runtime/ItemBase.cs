using UnityEngine;
namespace Toolkid.UIGrid {
    public class ItemBase : MonoBehaviour, IItemData {        
        public string Identifier { get => ItemData.Identifier; set => ItemData.Identifier = value; }
        public string Name { get => ItemData.Name; set => ItemData.Name = value; }
        public string Toolkit { get => ItemData.Toolkit; set => ItemData.Toolkit = value; }
        public ItemData ItemData { get => data; set => data = value; }

        [SerializeField] private ItemData data;
    }
}
