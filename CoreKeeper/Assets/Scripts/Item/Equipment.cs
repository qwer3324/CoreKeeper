using UnityEngine;
using UnityEngine.Events;

public class Equipment : SingletonBehaviour<Equipment>
{
    private Inventory inventory;
    private Appearance appearance;
    private Player player;

    private Item[] items;    //  장착된 아이템
    public Item[] Items { get { return items; } }

    /// <summary>아이템 장착 이벤트 함수</summary>
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
        //  잘못된 아이템이 들어왔을 때
        if (_newItem == null || _newItem.id < 0 || (int)GetItemType(_newItem) > (int)ItemType.Weapon)
            return;

        int index = (int)GetItemType(_newItem);

        Item equippedItem = items[index];

        //  이미 장착되어있는 아이템이 있으면
        if (equippedItem != null)
        {
            UnEquipItem(equippedItem);
            //inventory.AddItem(equippedItem);    //  템창이 꽉찬 상태에서 장착시 원래 아이템이 들어갈 원래 공간을 찾아서 바꿔줘야
        }

        //  장착
        items[index] = _newItem;

        //  외형 적용
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

        //  아이템 능력치 적용
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
        bool isSuccess = inventory.AddItem(_equippedItem);  //  장착 해제하는 아이템 인벤토리에 다시 넣기

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

            //  아이템 능력치 빼기
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
        bool isSuccess = inventory.AddItem(_equippedItem);  //  장착 해제하는 아이템 인벤토리에 다시 넣기

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
        //  [0]머리 / [1]상의 / [2]하의 / [3]목걸이 / [4]반지 / [5] 무기
        return inventory.itemDB.Datas[_item.id].type;
    }
}
