using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    private Equipment equipment;
    public GameObject itemUI;   //  Equipment UI

    private InventoryUI inventoryUI;
    private InformationUI informationUI;

    public Transform equipSlotsParent;
    private EquipSlot[] equipSlots;

    private int selectIndex = -1;   //  선택한 아이템 슬롯 인덱스
    public int SelectIndex { get { return selectIndex; }  }

    private void Start()
    {
        equipment = Equipment.Instance;
        equipment.OnEquipmentChanged += UpdateEquipmentUI;

        inventoryUI = UIManager.Instance.GetUI(UIManager.UI_Type.Inventory).GetComponent<InventoryUI>();
        informationUI = UIManager.Instance.GetUI(UIManager.UI_Type.Information).GetComponent<InformationUI>();

        equipSlots = equipSlotsParent.GetComponentsInChildren<EquipSlot>();
        UpdateEquipmentUI();
    }

    private void OnDisable()
    {
        DeselectEquipSlot();
    }

    private void UpdateEquipmentUI()
    {
        if (itemUI.activeSelf == false)
            return;

        //모든 슬롯 초기화
            for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].ResetEquipSlot();
        }

        //모든 슬롯 장착 아이템 배열 목록에서 가져와 셋팅
        for (int i = 0; i < equipment.Items.Length; i++)
        {
            equipSlots[i].SetEquipSlot(equipment.Items[i], i);
        }
    }

    public void SelectEquipSlot(int _index)
    {
        inventoryUI.DeselectItemSlot();

        if(selectIndex == _index)
        {
            DeselectEquipSlot();
            return;
        }

        DeselectEquipSlot();

        selectIndex = _index;

        equipSlots[selectIndex].selectImage.SetActive(true);

        informationUI.gameObject.SetActive(true);
        informationUI.UpdateItemInfo(equipment.Items[selectIndex]);
    }

    public void DeselectEquipSlot()
    {
        if (selectIndex < 0) return;

        equipSlots[selectIndex].selectImage.SetActive(false);
        selectIndex = -1;

        //  아이템 정보창 닫기 구현
        informationUI.UpdateItemInfo(null);
        informationUI.gameObject.SetActive(false);
    }

    private void DeselectSlotAll()
    {
        for(int i = 0; i < equipSlots.Length; i++) 
        {
            equipSlots[i].selectImage.SetActive(false);
        }

        selectIndex = -1;
    }
}
