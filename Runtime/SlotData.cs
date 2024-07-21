using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class SlotData : ITimeLog {
    public string ItemId { get => itemId; private set => itemId = value; }
    public RawImage Image { get => image; private set => image = value; }
    public bool HasUsed { get => hasUsed; private set => hasUsed = value; }
    public DateTime FirstTime { get => firstTime; set => firstTime = value; }
    public DateTime FinalTime { get => finalTime; set => finalTime = value; }
    public int CenterIndex { get => centerIndex; set => centerIndex = value; }

    [SerializeField, FormerlySerializedAs("itemIndex")] private string itemId = string.Empty;
    [SerializeField] private Texture originTexture;
    [SerializeField] private Color originColor;
    [SerializeField, FormerlySerializedAs("m_Image")] private RawImage image;
    [SerializeField] private bool hasUsed = false;
    [SerializeField] private int centerIndex = -1;
    public DateTime firstTime;
    public DateTime finalTime;


    public SlotData (RawImage image) {
        Image = image;
        originTexture = image.texture;
        originColor = image.color;
    }

    public void Reset() {
        Image.texture = originTexture;
        Image.color = originColor;
    }

    public void Normalize() {
        if (!hasUsed) {
            Reset();
        }
        else {
            // Get item image with itemIndex.
            Image.color = Color.blue;
        }
    }

    public void SetData(string itemId) {
        ItemId = itemId;
    }

    public void SetData(Color color) {
        Image.color = color;
    }

    public void SetData (Texture skin) {
        Image.texture = skin;
    }

    public void Build(int center, string itemId) {
        HasUsed = true;
        centerIndex = center;
        ItemId = itemId;
        FirstTime = DateTime.Now;
        FinalTime = FirstTime;
    }

    public void Store(int center) {
        if (hasUsed) {
            centerIndex = center;
            FinalTime = DateTime.Now;
        }
        else {
            Debug.LogWarning("`SlotData` has not used.");
        }
    }
    public void Clear(int index) {
        HasUsed = false;
        centerIndex = -1;
    }
}

public interface ITimeLog {
    DateTime FirstTime { get; set; }
    DateTime FinalTime { get; set; }
}
