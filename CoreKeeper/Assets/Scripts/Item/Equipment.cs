using UnityEngine;
using UnityEngine.Events;

public class Equipment : SingletonBehaviour<Equipment>
{
    private Inventory inventory;
    private Appearance appearance;
    private Player player;

    private Item[] items;    //  ������ ������
    public Item[] Items { get { return items; } }

    /// <summary>������ ���� �̺�Ʈ �Լ�</summary>
    public UnityAction OnEquipmentChanged;

    private void Start()
    {
        inventory = Inventory.Instance;

        appearance = GameObject.FindGameObjectWithTag("Player").GetComponent<Appearance>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        items = new Item[6];
    }

    public void EquipItem(Item _newItem)
    {
        //  �߸��� �������� ������ ��
        if (_newItem == null || _newItem.id < 0 || (int)GetItemType(_newItem) > (int)ItemType.Weapon)
            return;

        int index = (int)GetItemType(_newItem);

        Item equippedItem = items[index];

        //  �̹� �����Ǿ��ִ� �������� ������
        if (equippedItem != null)
        {
            UnEquipItem(equippedItem);
            //inventory.AddItem(equippedItem);    //  ��â�� ���� ���¿��� ������ ���� �������� �� ���� ������ ã�Ƽ� �ٲ����
        }

        //  ����
        items[index] = _newItem;

        //  ���� ����
        switch (inventory.itemDB.Datas[items[index].id].type)
        {
            case ItemType.Helm:
                appearance.SetAppearanceData(Appearance.PlayerPart.Head, inventory.itemDB.Datas[items[index].id].appearance);
                break;
            case ItemType.Chest:
                appearance.SetAppearanceData(Appearance.PlayerPart.Top, inventory.itemDB.Datas[items[index].id].appearance);
                break;
            case ItemType.Pants:
                appearance.SetAppearanceData(Appearance.PlayerPart.Bottom, inventory.itemDB.Datas[items[index].id].appearance);
                break;
            case ItemType.Weapon:
                appearance.SetAppearanceData(Appearance.PlayerPart.Holder, inventory.itemDB.Datas[items[index].id].appearance);
                break;
        }

        //  ������ �ɷ�ġ ����
        for(int i = 0; i < items[index].abilities.Length; ++i)
        {
            switch (items[index].abilities[i].type)
            {
                case AttributeType.MaxHealth:
                    player.MaxHealth += items[index].abilities[i].value;
                    break;
                case AttributeType.Armor:
                    player.DefencePower += items[index].abilities[i].value;
                    break;
                case AttributeType.DodgeChance:
                    player.DodgeChance += items[index].abilities[i].value;
                    break;
                case AttributeType.CriticalHitDamage:
                    player.CriticalHitDamage += items[index].abilities[i].value * 0.01f;
                    break;
                case AttributeType.MeleeDamage:
                    if(index == (int)ItemType.Weapon)
                    {
                        player.WeaponDamage = items[index].abilities[i].value;
                    }
                    else
                    {
                        player.EquipDamage += items[index].abilities[i].value;
                    }
                    break;
                case AttributeType.RangeDamage:
                    if (index == (int)ItemType.Weapon)
                    {
                        player.WeaponDamage = items[index].abilities[i].value;
                        player.ProjectilePrefab = WeaponManager.Instance.GetProjectile(items[index].name);
                    }
                    else
                    {
                        player.EquipDamage += items[index].abilities[i].value;
                    }
                    break;
                case AttributeType.CriticalHitChance:
                    player.CriticalHitChance += items[index].abilities[i].value;
                    break;
            }
        }

        OnEquipmentChanged?.Invoke();
    }

    public bool UnEquipItem(Item _equippedItem)
    {
        bool isSuccess = inventory.AddItem(_equippedItem);  //  ���� �����ϴ� ������ �κ��丮�� �ٽ� �ֱ�

        if (isSuccess)
        {
            int index = (int)GetItemType(_equippedItem);

            switch (GetItemType(_equippedItem))
            {
                case ItemType.Helm:
                    appearance.SetAppearanceData(Appearance.PlayerPart.Head, null);
                    break;
                case ItemType.Chest:
                    appearance.SetAppearanceData(Appearance.PlayerPart.Top, null);
                    break;
                case ItemType.Pants:
                    appearance.SetAppearanceData(Appearance.PlayerPart.Bottom, null);
                    break;
                case ItemType.Weapon:
                    appearance.SetAppearanceData(Appearance.PlayerPart.Holder, null);
                    break;
            }

            //  ������ �ɷ�ġ ����
            for (int i = 0; i < items[index].abilities.Length; ++i)
            {
                switch (items[index].abilities[i].type)
                {
                    case AttributeType.MaxHealth:
                        player.MaxHealth -= items[index].abilities[i].value;
                        break;
                    case AttributeType.Armor:
                        player.DefencePower -= items[index].abilities[i].value;
                        break;
                    case AttributeType.DodgeChance:
                        player.DodgeChance -= items[index].abilities[i].value;
                        break;
                    case AttributeType.CriticalHitDamage:
                        player.CriticalHitDamage -= items[index].abilities[i].value * 0.01f;
                        break;
                    case AttributeType.MeleeDamage:
                        if (index == (int)ItemType.Weapon)
                        {
                            player.WeaponDamage = 0;
                        }
                        else
                        {
                            player.EquipDamage -= items[index].abilities[i].value;
                        }
                        break;
                    case AttributeType.RangeDamage:
                        if(index == (int)ItemType.Weapon)
                        {
                            player.WeaponDamage = 0;
                            player.ProjectilePrefab = null;
                        }
                        else
                        {
                            player.EquipDamage -= items[index].abilities[i].value;
                        }
                        break;
                    case AttributeType.CriticalHitChance:
                        player.CriticalHitChance -= items[index].abilities[i].value;
                        break;
                }
            }

            items[index] = null;

            OnEquipmentChanged?.Invoke();
        }

        return isSuccess;
    }

    public bool UnEquipItem(Item _equippedItem, int _invenIndex)
    {
        bool isSuccess = inventory.AddItem(_equippedItem);  //  ���� �����ϴ� ������ �κ��丮�� �ٽ� �ֱ�

        if (isSuccess)
        {
            int index = (int)GetItemType(_equippedItem);
            items[index] = null;

            OnEquipmentChanged?.Invoke();
        }

        return isSuccess;
    }

    public ItemType GetItemType(Item _item)
    {
        //  [0]�Ӹ� / [1]���� / [2]���� / [3]����� / [4]���� / [5] ����
        return inventory.itemDB.Datas[_item.id].type;
    }
}
