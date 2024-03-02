using System;
using Toolkid.GridInventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Placeables : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {    
    [SerializeField] GameObject m_dragging;
    [SerializeField] Image m_Image;
    [SerializeField] PlaceablesData m_PlaceablesDatas;
    public PlaceablesData PlaceablesDatas { get => m_PlaceablesDatas; set => m_PlaceablesDatas = value; }
    public event EventHandler<PointerEventData> OnDragBegin;
    public event EventHandler<PointerEventData> OnDragEnd;

    public void OnBeginDrag(PointerEventData eventData) {
        m_dragging = Instantiate(m_Image.gameObject, InventoryManager.Current.GridDrawer.transform.parent);
        Canvas canvas = m_dragging.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 2;
        m_dragging.transform.localPosition = Vector3.zero;
        InventoryManager.Current.OnPlaceable(m_PlaceablesDatas.Sharp);
        OnDragBegin?.Invoke(this, eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(InventoryManager.Current.GridSystem.Rect, eventData.position, Camera.main, out Vector3 maskPos);
        InventoryManager.Current.OnHover(maskPos);             
        m_dragging.transform.position = maskPos;
    }

    public void OnEndDrag(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(InventoryManager.Current.GridSystem.GetComponent<RectTransform>(), eventData.position, Camera.main, out Vector3 maskPos);
        InventoryManager.Current.OnHover(maskPos);
        DestroyImmediate(m_dragging.gameObject);
        InventoryManager.Current.OnPlace();
        OnDragEnd?.Invoke(this, eventData);
    }
}
