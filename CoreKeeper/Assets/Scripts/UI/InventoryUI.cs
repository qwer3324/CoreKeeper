using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region ����
    private Inventory inventory;
    private InformationUI informationUI;
    public Transform itemSlotsParent;       //  ������ ���Ե��� �θ�
    public Button trashCanBtn;
    #endregion

    private ItemSlot[] itemSlots;
    private int selectIndex = -1;   //  ������ ������ ���� �ε���

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

    //  Inventory UI ����
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
        //  ��� UI�� �̹� ���õǾ� �ִ� ������ �ִٸ�
        if (UIManager.Instance.GetUI(UIManager.UI_Type.Equipment).GetComponent<EquipmentUI>().SelectIndex >= 0)
        {
            UIManager.Instance.GetUI(UIManager.UI_Type.Equipment).GetComponent<EquipmentUI>().DeselectEquipSlot();
        }

        if (selectIndex == _index)
        {
            //  ���� ������ ����
            DeselectItemSlot();
            return;
        }
        else if (selectIndex >= 0) DeselectItemSlot();

        //  ������ ����â ����
        if (itemSlots[_index].Item != null)
        {
            selectIndex = _index;
            itemSlots[selectIndex].selectImage.SetActive(true);
            informationUI.gameObject.SetActive(true);
            informationUI.UpdateItemInfo(itemSlots[selectIndex].Item);

            //  ������ Ȱ��ȭ
            trashCanBtn.interactable = true;
        }
    }

    public void DeselectItemSlot()
    {
        //  ������ ����â �ݱ� ����
        informationUI.UpdateItemInfo(null);
        informationUI.gameObject.SetActive(false);

        //  ������ ��Ȱ��ȭ
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
