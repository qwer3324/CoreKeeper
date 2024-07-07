using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    private Equipment equipment;
    public GameObject itemUI;   //  Equipment UI

    private InventoryUI inventoryUI;
    private InformationUI informationUI;

    public Transform equipSlotsParent;
    private EquipSlot[] equipSlots;

    private int selectIndex = -1;   //  ������ ������ ���� �ε���
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

        //��� ���� �ʱ�ȭ
            for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].ResetEquipSlot();
        }

        //��� ���� ���� ������ �迭 ��Ͽ��� ������ ����
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

        //  ������ ����â �ݱ� ����
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
