using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region 참조
    private Inventory inventory;
    private InformationUI informationUI;
    public Transform itemSlotsParent;       //  아이템 슬롯들의 부모
    public Button trashCanBtn;
    #endregion

    private ItemSlot[] itemSlots;
    private int selectIndex = -1;   //  선택한 아이템 슬롯 인덱스

    private void Start()
    {
        inventory = Inventory.Instance;
        inventory.OnItemChanged += UpdateInventoryUI;
        inventory.OnItemChanged += DeselectItemSlot;

        itemSlots = itemSlotsParent.GetComponentsInChildren<ItemSlot>();

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].Index = i;
        }

        UpdateInventoryUI();

        informationUI = UIManager.Instance.GetUI(UIManager.UI_Type.Information).GetComponent<InformationUI>();
        trashCanBtn.interactable = false;
    }

    private void OnDisable()
    {
        DeselectItemSlot();
    }

    //  Inventory UI 갱신
    private void UpdateInventoryUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].RemoveItemInSlot();
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].SetItemInSlot(inventory.Items[i]);
        }
    }

    public void SelectItemSlot(int _index)
    {
        //  장비 UI에 이미 선택되어 있는 슬롯이 있다면
        if (UIManager.Instance.GetUI(UIManager.UI_Type.Equipment).GetComponent<EquipmentUI>().SelectIndex >= 0)
        {
            UIManager.Instance.GetUI(UIManager.UI_Type.Equipment).GetComponent<EquipmentUI>().DeselectEquipSlot();
        }

        if (selectIndex == _index)
        {
            //  같은 슬롯을 선택
            DeselectItemSlot();
            return;
        }
        else if (selectIndex >= 0) DeselectItemSlot();

        //  아이템 정보창 열기
        if (itemSlots[_index].Item != null)
        {
            selectIndex = _index;
            itemSlots[selectIndex].selectImage.SetActive(true);
            informationUI.gameObject.SetActive(true);
            informationUI.UpdateItemInfo(itemSlots[selectIndex].Item);

            //  버리기 활성화
            trashCanBtn.interactable = true;
        }
    }

    public void DeselectItemSlot()
    {
        //  아이템 정보창 닫기 구현
        informationUI.UpdateItemInfo(null);
        informationUI.gameObject.SetActive(false);

        //  버리기 비활성화
        trashCanBtn.interactable = false;

        if (selectIndex < 0) return;

        itemSlots[selectIndex].selectImage.SetActive(false);
        selectIndex = -1;
    }

    //public void UseItem()
    //{
    //    inventory.Items[selectIndex].Use();
    //    inventory.RemoveItem(inventory.Items[selectIndex], selectIndex);

    //    if(selectIndex >= 0)
    //        DeselectItemSlot();
    //}

    public void DeleteItemSlot()
    {
        if(selectIndex >= 0) 
        {
            inventory.DeleteItem(selectIndex);
            DeselectItemSlot();
        }
    }
}
