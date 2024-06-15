using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class SlotData : ITimeLog {
    [SerializeField] private int itemIndex = -1;
    [SerializeField] private Texture originTexture;
    [SerializeField] private Color originColor;
    [SerializeField, FormerlySerializedAs("m_Image")] private RawImage image;
    [SerializeField] private bool hasUsed = false;
    [SerializeField] private int centerIndex = -1;
    public DateTime firstTime;
    public DateTime finalTime;
    public int ItemIndex { get => itemIndex; private set => itemIndex = value; }
    public RawImage Image { get => image; private set => image = value; }
    public bool HasUsed { get => hasUsed; private set => hasUsed = value; }
    public DateTime FirstTime { get => firstTime; set => firstTime = value; }
    public DateTime FinalTime { get => finalTime; set => finalTime = value; }
    public int CenterIndex { get => centerIndex; set => centerIndex = value; }

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

    public void SetData(int itemIndex) {
        ItemIndex = itemIndex;
    }

    public void SetData(Color color) {
        Image.color = color;
    }

    public void SetData (Texture skin) {
        Image.texture = skin;
    }

    public void Build(int center, int item) {
        HasUsed = true;
        centerIndex = center;
        ItemIndex = item;
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
