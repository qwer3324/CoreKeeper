using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    #region ����
    private Inventory inventory;
    private InventoryUI inventoryUI;
    #endregion

    //  ������ �ִ� ������
    private Item item;
    public Item Item { get { return item; } }

    public GameObject iconImage;        //  ������ �̹��� ����ִ� ������Ʈ
    public GameObject selectImage;
    public TextMeshProUGUI amountText;

    private int index;
    public int Index { get { return index; }  set { index = value; } }

    private void Awake()
    {
        inventory = Inventory.Instance;
        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void SetItemInSlot(Item _item)
    {
        if (_item == null || _item.id < 0)
        {
            RemoveItemInSlot();
            return;
        }

        item = _item;
        iconImage.SetActive(true);
        iconImage.GetComponent<Image>().sprite = inventory.itemDB.Datas[_item.id].icon;
        if (_item.amount > 0) amountText.text = _item.amount.ToString();
        else amountText.text = "";
    }

    public void RemoveItemInSlot()
    {
        item = null;
        iconImage.SetActive(false);
        iconImage.GetComponent<Image>().sprite = null;
        amountText.text = "";
    }

    public void Selected()
    {
        inventoryUI.SelectItemSlot(index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item != null)
            {
                if(inventory.itemDB.Datas[item.id].type <= ItemType.Weapon)
                {
                    //  ���
                    Equipment.Instance.EquipItem(Item);
                    Inventory.Instance.RemoveItem(item, index);
                }
                else if (inventory.itemDB.Datas[item.id].type <= ItemType.Potion)
                {
                    //  �����
                    Inventory.Instance.UseItem(Item, index);
                }
                else if(inventory.itemDB.Datas[item.id].type <= ItemType.CraftingStation)
                {
                    UIManager.Instance.SetPlacedItem(Item, index);
                }
            }
        }
    }
}
