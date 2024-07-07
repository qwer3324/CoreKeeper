using UnityEngine;
using UnityEngine.EventSystems;

public class EquipDropMe : MonoBehaviour, IDropHandler
{
    #region Variables

    public EquipSlot m_EquipSlot;       //이동 받는 데이터(정보) 담는 그릇

    private int invenDropIndex = -1;    //인벤토리 목록의 인덱스
    #endregion

    //드래그중 마우스 포인트를 뗄떼 호출
    public void OnDrop(PointerEventData eventData)
    {
        //드랍한 아이템 처리
        bool isGetValue = GetInvenDropIndex(eventData);
        if (isGetValue)
        {
            //인벤토리 아이템을 장착하기
            if (invenDropIndex >= 0)
            {
                EquipmentEquipItem(invenDropIndex);
            }
        }
    }

    //드랍 이벤트로 부터 필요한 아이템 정보 가져오기
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

            //가져온 아이템의 장착 자리 찾기
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

        //아이템을 장착창에 장착
        Equipment.Instance.EquipItem(Inventory.Instance.Items[dropIndex]);
        //장착한 아이템을 인벤토리에서 제거
        Inventory.Instance.RemoveItem(Inventory.Instance.Items[dropIndex], dropIndex);
    }
}