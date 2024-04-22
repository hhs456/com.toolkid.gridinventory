using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class SlotData {
    [SerializeField] private Texture originTexture;
    [SerializeField] private Color originColor;
    [SerializeField, FormerlySerializedAs("m_Image")] private RawImage image;
    [SerializeField] private bool hasUsed = false;
    [SerializeField] private int centerIndex = -1;

    public RawImage Image { get => image; private set => image = value; }

    public bool HasUsed { get => hasUsed; private set => hasUsed = value; }

    public SlotData (RawImage image) {
        Image = image;
        originTexture = image.texture;
        originColor = image.color;
    }

    public void Reset() {
        Image.texture = originTexture;
        Image.color = originColor;
    }

    public void SetData(Color color) {
        Image.color = color;
    }

    public void SetData (Texture skin) {
        Image.texture = skin;
    }

    public void SetData(int center) {
        HasUsed = true;
        centerIndex = center;
    }
}
