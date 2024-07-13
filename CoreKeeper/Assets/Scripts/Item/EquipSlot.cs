using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    private Inventory inventory;
    private Equipment equipment;
    private EquipmentUI equipmentUI;

    private Item item;              //������ ������ �ִ� ������
    public Item Item {  get { return item; } }
    public GameObject iconImage;    //���� ������ ������ �̹��� ������Ʈ
    public GameObject selectImage;  //���� ���� �̹���

    private Sprite silhouetteSprite;

    private int slotIndex;
    public int Index {  get { return slotIndex; } }

    private void OnEnable()
    {
        if (inventory == null)
            inventory = Inventory.Instance;
        if (equipment == null)
            equipment = Equipment.Instance;
        if (equipmentUI == null)
            equipmentUI = GetComponentInParent<EquipmentUI>();
        if(silhouetteSprite == null)
            silhouetteSprite = iconImage.GetComponent<Image>().sprite;
    }

    public void SelectEquipSlot()
    {
        if (item == null || item.id < 0)
            return;

        //�ڽ��� ���� ���� �ε����� �Ѱ��ش�
        equipmentUI.SelectEquipSlot(slotIndex);
    }

    public void ResetEquipSlot()
    {
        item = null;
        iconImage.GetComponent<Image>().sprite = silhouetteSprite;

        selectImage.SetActive(false);
    }

    public void SetEquipSlot(Item _item, int _index)
    {
        slotIndex = _index;

        if (_item == null || _item.id < 0)
            return;

        item = _item;
        iconImage.GetComponent<Image>().sprite = inventory.itemDB.Datas[item.id].icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // ���⿡ ��Ŭ�� �� ������ ������ �߰�
            if (Item != null)
            {
                //���� �����Ǿ��ִ� ������ �κ��� ������
                Equipment.Instance.UnEquipItem(Item);
            }
        }
    }
}
