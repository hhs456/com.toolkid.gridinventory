using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Toolkid.UIGrid {
    public class Placeables : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
        [SerializeField] private GameObject dragging;
        [SerializeField] private RawImage image;
        [SerializeField] private int objectID;

        public event EventHandler<PointerEventData> OnDragBegin;
        public event EventHandler<PointerEventData> OnDragEnd;

        // Only for testing version.
        public void Initialize(int itemID) {
            objectID = itemID;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            dragging = Instantiate(image.gameObject, InventoryManager.Current.Validator.transform.parent);
            Canvas canvas = dragging.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 2;
            dragging.transform.localPosition = Vector3.zero;
            InventoryManager.Current.DoPlaceable(PlaceablesDatas.Datas[objectID].Sharp);
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
                if (pointer.TryGetComponent(out Stackables pointed)) {
                    if (pointed.Data.Index == objectID) {

                    }
                    // Stacks Behaviour
                }
            }
            
        }
    }
}
