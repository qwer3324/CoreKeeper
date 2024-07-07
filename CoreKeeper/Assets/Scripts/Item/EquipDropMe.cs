using UnityEngine;
using UnityEngine.EventSystems;

public class EquipDropMe : MonoBehaviour, IDropHandler
{
    #region Variables

    public EquipSlot m_EquipSlot;       //�̵� �޴� ������(����) ��� �׸�

    private int invenDropIndex = -1;    //�κ��丮 ����� �ε���
    #endregion

    //�巡���� ���콺 ����Ʈ�� ���� ȣ��
    public void OnDrop(PointerEventData eventData)
    {
        //����� ������ ó��
        bool isGetValue = GetInvenDropIndex(eventData);
        if (isGetValue)
        {
            //�κ��丮 �������� �����ϱ�
            if (invenDropIndex >= 0)
            {
                EquipmentEquipItem(invenDropIndex);
            }
        }
    }

    //��� �̺�Ʈ�� ���� �ʿ��� ������ ���� ��������
    bool GetInvenDropIndex(PointerEventData eventData)
    {
        bool retValue = false;
        invenDropIndex = -1;

        var originalObj = eventData.pointerDrag;
        if (originalObj == null)
            return false;

        var invenDrag = originalObj.GetComponent<InvenDragMe>();
        if (invenDrag && invenDrag.itemSlot)
        {
            int dropIndex = invenDrag.itemSlot.Index;

            //������ �������� ���� �ڸ� ã��
            if (dropIndex >= 0)
            {
                int index = (int)Equipment.Instance.GetItemType(Inventory.Instance.Items[dropIndex]);
                if (m_EquipSlot.Index == index)
                {
                    invenDropIndex = dropIndex;
                    retValue = true;
                }
            }
        }

        return retValue;
    }

    void EquipmentEquipItem(int dropIndex)
    {
        if (dropIndex < 0)
            return;

        //�������� ����â�� ����
        Equipment.Instance.EquipItem(Inventory.Instance.Items[dropIndex]);
        //������ �������� �κ��丮���� ����
        Inventory.Instance.RemoveItem(Inventory.Instance.Items[dropIndex], dropIndex);
    }
}