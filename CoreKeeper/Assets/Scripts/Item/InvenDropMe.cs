using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Drag한 게임오브젝트(UI) 을 받는것을 구현
public class InvenDropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Variables
    public Image containerImage;

    public Color highlightColor = Color.red;
    private Color originColor;

    private ItemSlot m_ItemSlot;
    #endregion

    private void OnEnable()
    {
        m_ItemSlot = ComponentHelper.FindInParents<ItemSlot>(gameObject);

        if (containerImage != null)
        {
            originColor = containerImage.color;
        }
    }


    //드래그중 마우스 포인트를 뗄떼 호출
    public void OnDrop(PointerEventData eventData)
    {
        if (containerImage == null)
            return;

        containerImage.color = originColor;

        //아이템 바꾸기
        int invenDropIndex = GetInvenDropIndex(eventData);
        InventorySwapItems(invenDropIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_ItemSlot == null || m_ItemSlot.Index < 0)
            return;

        if (containerImage == null)
            return;

        //바꿀것이 있으면 색 변화 - 아이콘을 드래그 하고 있으면
        int invenDropIndex = GetInvenDropIndex(eventData);
        if (invenDropIndex > -1)
        {
            containerImage.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (containerImage == null)
            return;

        containerImage.color = originColor;
    }

    //드랍 이벤트로 부터 아이템슬롯 인덱스 가져오기
    int GetInvenDropIndex(PointerEventData eventData)
    {
        var originalObj = eventData.pointerDrag;
        if (originalObj == null)
            return -1;

        var dragMe = originalObj.GetComponent<InvenDragMe>();
        if (dragMe == null)
            return -1;

        var itemSlot = dragMe.itemSlot;
        if (itemSlot == null)
            return -1;

        return itemSlot.Index;
    }

    void InventorySwapItems(int dropIndex)
    {
        if (dropIndex < 0)
            return;

        if (m_ItemSlot == null || m_ItemSlot.Index < 0)
            return;

        Inventory.Instance.SwapItems(dropIndex, m_ItemSlot.Index);
    }
}