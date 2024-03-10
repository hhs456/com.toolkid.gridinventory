using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SlotData {
    [SerializeField] private Sprite originTexture;
    [SerializeField] private Color originColor;
    [SerializeField] private Image m_Image;
    [SerializeField] private bool hasUsed = false;
    [SerializeField] private int centerIndex = -1;

    public Image Image { get => m_Image; private set => m_Image = value; }

    public bool HasUsed { get => hasUsed; private set => hasUsed = value; }

    public SlotData (Image image) {
        Image = image;
        originTexture = image.sprite;
        originColor = image.color;
    }

    public void Reset() {
        Image.sprite = originTexture;
        Image.color = originColor;
    }

    public void SetData(Color color) {
        Image.color = color;
    }

    public void SetData (Sprite skin) {
        Image.sprite = skin;
    }

    public void SetData(int center) {
        HasUsed = true;
        centerIndex = center;
    }
}
