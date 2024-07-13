using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//게임오브젝트(UI) Drag 구현
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

    //드래그 시작시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot == null || itemSlot.Item == null || itemSlot.Item.id < 0) return;

        //클릭한 게임 오브젝트의 부모에서 캔바스 컴포넌트 찾기
        var canvas = ComponentHelper.FindInParents<Canvas>(this.gameObject);

        if (canvas == null)
            return;

        //마우스 포인터 붙어 다니는 아이콘 오브젝트 생성
        m_DraggingIcons[eventData.pointerId] = new GameObject("icon");

        //아이콘 오브젝트 canvas의 자식으로 넣는다
        m_DraggingIcons[eventData.pointerId].transform.SetParent(canvas.transform, false);
        m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();

        //아이콘 오브젝트 이미지 추가
        var image = m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
        var group = m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;

        image.sprite = itemSlot.iconImage.GetComponent<Image>().sprite;
        image.SetNativeSize();

        m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;
        SetDraggedPosition(eventData);
    }

    //드래그 ing
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

    //드래그 끝날때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons.ContainsKey(eventData.pointerId))
            Destroy(m_DraggingIcons[eventData.pointerId]);

        m_DraggingIcons[eventData.pointerId] = null;
    }
}
