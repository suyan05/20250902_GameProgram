using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventorySlot slot;
    private Transform originalParent;
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var target = eventData.pointerEnter?.GetComponent<InventorySlot>();
        if (target != null && target != slot)
        {
            // Swap items
            var tempType = slot.type;
            var tempCount = slot.count;

            slot.SetItem(target.type, target.count);
            target.SetItem(tempType, tempCount);
        }

        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
    }
}