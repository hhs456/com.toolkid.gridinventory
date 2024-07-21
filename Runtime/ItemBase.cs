using UnityEngine;
namespace Toolkid.UIGrid {
    public class ItemBase : MonoBehaviour, IItemData {        
        public int Index { get => ItemData.Index; set => ItemData.Index = value; }
        public string Name { get => ItemData.Name; set => ItemData.Name = value; }
        public string Toolkit { get => ItemData.Toolkit; set => ItemData.Toolkit = value; }
        public ItemData ItemData { get => data; set => data = value; }

        [SerializeField] private ItemData data;
    }
}
