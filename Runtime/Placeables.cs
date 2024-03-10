using System;
using Toolkid.GridInventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Toolkid.GridInventory {
    public class Placeables : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
        [SerializeField] GameObject m_dragging;
        [SerializeField] Image m_Image;
        [SerializeField] int m_ObjectID;
        public event EventHandler<PointerEventData> OnDragBegin;
        public event EventHandler<PointerEventData> OnDragEnd;

        // Only for testing version.
        public void Initialize(int itemID) {
            m_ObjectID = itemID;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            m_dragging = Instantiate(m_Image.gameObject, InventoryManager.Current.GridDrawer.transform.parent);
            Canvas canvas = m_dragging.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 2;
            m_dragging.transform.localPosition = Vector3.zero;
            InventoryManager.Current.OnPlaceable(InventoryManager.Current.Placeables[m_ObjectID].Sharp);
            OnDragBegin?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(InventoryManager.Current.GridSystem.Rect, eventData.position, Camera.main, out Vector3 cursorPos);
            InventoryManager.Current.OnHover(cursorPos);
            m_dragging.transform.position = cursorPos;
        }

        public void OnEndDrag(PointerEventData eventData) {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(InventoryManager.Current.GridSystem.GetComponent<RectTransform>(), eventData.position, Camera.main, out Vector3 cursorPos);
            InventoryManager.Current.OnPlace(cursorPos);
            DestroyImmediate(m_dragging.gameObject);
            OnDragEnd?.Invoke(this, eventData);
            var pointer = eventData.pointerCurrentRaycast.gameObject;
            if (pointer) {
                if (pointer.TryGetComponent(out Stackables pointed)) {
                    if (pointed.Data.ID == m_ObjectID) {

                    }
                    // Stacks Behaviour
                }
            }
            
        }
    }
}
