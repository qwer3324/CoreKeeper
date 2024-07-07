using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//���ӿ�����Ʈ(UI) Drag ����
public class InvenDragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variables
    private Dictionary<int, GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
    private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

    public ItemSlot itemSlot;
    #endregion

    private void OnEnable()
    {
        itemSlot = ComponentHelper.FindInParents<ItemSlot>(gameObject);
    }

    //�巡�� ���۽� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot == null || itemSlot.Item == null || itemSlot.Item.id < 0) return;

        //Ŭ���� ���� ������Ʈ�� �θ𿡼� ĵ�ٽ� ������Ʈ ã��
        var canvas = ComponentHelper.FindInParents<Canvas>(this.gameObject);

        if (canvas == null)
            return;

        //���콺 ������ �پ� �ٴϴ� ������ ������Ʈ ����
        m_DraggingIcons[eventData.pointerId] = new GameObject("icon");

        //������ ������Ʈ canvas�� �ڽ����� �ִ´�
        m_DraggingIcons[eventData.pointerId].transform.SetParent(canvas.transform, false);
        m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();

        //������ ������Ʈ �̹��� �߰�
        var image = m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
        var group = m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;

        image.sprite = itemSlot.iconImage.GetComponent<Image>().sprite;
        image.SetNativeSize();

        m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;
        SetDraggedPosition(eventData);
    }

    //�巡�� ing
    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot == null || itemSlot.Item == null || itemSlot.Item.id < 0) return;

        if (m_DraggingIcons.ContainsKey(eventData.pointerId))
        {
            SetDraggedPosition(eventData);
        }
    }

    void SetDraggedPosition(PointerEventData eventData)
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

    //�巡�� ������ ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons.ContainsKey(eventData.pointerId))
            Destroy(m_DraggingIcons[eventData.pointerId]);

        m_DraggingIcons[eventData.pointerId] = null;
    }
}
