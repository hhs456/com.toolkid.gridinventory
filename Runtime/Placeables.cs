using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Toolkid.UIGrid {
    public class Placeables : Stackables, IDragHandler, IBeginDragHandler, IEndDragHandler {
        public static GameObject Dragging { get; private set; }
      
        [SerializeField] private GameObject dragging;
        [SerializeField] private RawImage image;        

        public event EventHandler<PointerEventData> OnDragBegin;
        public event EventHandler<PointerEventData> OnDragEnd;
       

        public void OnBeginDrag(PointerEventData eventData) {
            dragging = Instantiate(image.gameObject, InventoryManager.Current.Validator.transform.parent);
            Dragging = dragging;
            Canvas canvas = dragging.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 2;
            dragging.transform.localPosition = Vector3.zero;
            InventoryManager.Current.DoPlaceable(Identifier);
            OnDragBegin?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(InventoryManager.Current.GridRegion.Rect, eventData.position, Camera.main, out Vector3 cursorPos);
            InventoryManager.Current.OnDragging(cursorPos);
            dragging.transform.position = cursorPos;
        }

        public void OnEndDrag(PointerEventData eventData) {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(InventoryManager.Current.GridRegion.GetComponent<RectTransform>(), eventData.position, Camera.main, out Vector3 cursorPos);
            Vector2Int gridIndex = InventoryManager.Current.GridRegion.GetIndex(cursorPos);
            InventoryManager.Current.Validator.PlaceOn(gridIndex);
            DestroyImmediate(dragging.gameObject);
            OnDragEnd?.Invoke(this, eventData);
            var pointer = eventData.pointerCurrentRaycast.gameObject;
            if (pointer) {
                //if (pointer.TryGetComponent(out Stackables pointed)) {
                //    if (pointed.Data.Index == index) {

                //    }
                //    // Stacks Behaviour
                //}
            }
            Dragging = null;
        }
    }
}
