using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Drag�� ���ӿ�����Ʈ(UI) �� �޴°��� ����
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


    //�巡���� ���콺 ����Ʈ�� ���� ȣ��
    public void OnDrop(PointerEventData eventData)
    {
        if (containerImage == null)
            return;

        containerImage.color = originColor;

        //������ �ٲٱ�
        int invenDropIndex = GetInvenDropIndex(eventData);
        InventorySwapItems(invenDropIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_ItemSlot == null || m_ItemSlot.Index < 0)
            return;

        if (containerImage == null)
            return;

        //�ٲܰ��� ������ �� ��ȭ - �������� �巡�� �ϰ� ������
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

    //��� �̺�Ʈ�� ���� �����۽��� �ε��� ��������
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