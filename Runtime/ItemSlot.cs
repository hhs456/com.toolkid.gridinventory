using System;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Toolkid.UIGrid {


    public interface IItemSlot {
        string ItemId { get; set; }
        RawImage Image { get; set; }
    }


    public interface ITimeLog {
        DateTime FirstTime { get; set; }
        DateTime FinalTime { get; set; }
    }
    [Serializable]
    public class ItemSlot : IItemSlot,  ITimeLog {        
        public int SlotIndex { get => slotIndex; set => slotIndex = value; }
        public string ItemId { get => itemId; set => itemId = value; }
        public RawImage Image { get => image; set => image = value; }
        public bool HasUsed { get => hasUsed; protected set => hasUsed = value; }
        public DateTime FirstTime { get => firstTime; set => firstTime = value; }
        public DateTime FinalTime { get => finalTime; set => finalTime = value; }

        [SerializeField]
        private int slotIndex;
        [SerializeField, FormerlySerializedAs("itemIndex")] private string itemId = string.Empty;
        [SerializeField] private Texture originTexture;
        [SerializeField] private Color originColor;
        [SerializeField, FormerlySerializedAs("m_Image")] private RawImage image;
        [SerializeField] private bool hasUsed = false;        
        public DateTime firstTime;
        public DateTime finalTime;


        public ItemSlot(RawImage image) {
            Image = image;
            originTexture = image.texture;
            originColor = image.color;
        }

        public void Reset() {
            slotIndex = -1;
            itemId = string.Empty;
            Image.texture = originTexture;
            Image.color = originColor;
            hasUsed = false;
        }

        public void Normalize() {
            if (!hasUsed) {
                Reset();
            }
            else {
                // Get item image with itemIndex.
                Image.color = originColor;
            }
        }

        public void SetData(string itemId) {
            ItemId = itemId;
        }

        public void SetData(Color color) {
            Image.color = color;
        }

        public void SetData(Texture skin) {
            Image.texture = skin;
        }

        public void Build(int slotIndex, string itemId) {
            HasUsed = true;            
            SlotIndex = slotIndex;
            ItemId = itemId;
            FirstTime = DateTime.Now;
            FinalTime = FirstTime;
        }
    }
}