using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    private Inventory inventory;
    private Equipment equipment;
    private EquipmentUI equipmentUI;

    private Item item;              //슬롯이 가지고 있는 아이템
    public Item Item {  get { return item; } }
    public GameObject iconImage;    //슬롯 아이템 아이콘 이미지 오브젝트
    public GameObject selectImage;  //슬롯 선택 이미지

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

        //자신의 고유 슬롯 인덱스를 넘겨준다
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
            // 여기에 우클릭 시 실행할 동작을 추가
            if (Item != null)
            {
                //원래 장착되어있던 아이템 인벤에 보내기
                Equipment.Instance.UnEquipItem(Item);
            }
        }
    }
}
