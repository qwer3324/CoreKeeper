using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipDragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Dictionary<int, GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
    private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

    public EquipSlot equipSlot;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equipSlot == null || equipSlot.Item == null || equipSlot.Item.id < 0) return;

        var canvas = ComponentHelper.FindInParents<Canvas>(gameObject);

        if (canvas == null) return;

        m_DraggingIcons[eventData.pointerId] = new GameObject("icon");

        m_DraggingIcons[eventData.pointerId].transform.SetParent(canvas.transform, false);
        m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();

        var image = m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
        var group = m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
        group.blocksRaycasts = true;

        image.sprite = equipSlot.iconImage.GetComponent<Image>().sprite;
        image.SetNativeSize();

        m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (equipSlot == null || equipSlot.Item == null || equipSlot.Item.id < 0) return;

            if (equipSlot == null) return;

            if (m_DraggingIcons.ContainsKey(eventData.pointerId))
            {
                SetDraggedPosition(eventData);
            }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons.ContainsKey(eventData.pointerId))
            Destroy(m_DraggingIcons[eventData.pointerId]);

        m_DraggingIcons[eventData.pointerId] = null;
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
            m_DraggingPlanes[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcons[eventData.pointerId].GetComponent<RectTransform>();

        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position,
            eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlanes[eventData.pointerId].rotation;
        }
    }
}
