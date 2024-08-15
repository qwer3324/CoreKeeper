using UnityEngine;
using UnityEngine.Events;

public class Inventory : SingletonBehaviour<Inventory>
{
    #region 인벤토리
    private Item[] items; // 아이템 배열
    public Item[] Items { get { return items; } }

    [SerializeField] private int maxCapacity = 30;     // 인벤토리 수용량
    public int MaxCapacity { get { return maxCapacity; } }

    [SerializeField] private int itemCount = 0;           // 인벤토리에 아이템이 들어있는 개수
    public int ItemCount { get { return itemCount; } }
    #endregion

    //  stackable에서 가장 높게 쌓을 수 있는 스택 개수
    [SerializeField] private int maxStack = 100;
    public ItemDB itemDB;
    public GameObject dropItemPrefab;

    /// <summary>인벤토리 아이템 변경시 호출</summary>
    public UnityAction OnItemChanged;

    private Player player;

    private new void Awake()
    {
        base.Awake();

        items = new Item[maxCapacity];

        player = FindObjectOfType<Player>();
    }

    public bool IsEmpty(Item _newItem)
    {
        bool empty = ItemCount < maxCapacity;

        if (!empty)
        {
            if (itemDB.Datas[_newItem.id].stackable)
            {
                foreach (Item item in items)
                {
                    if (item == null || item.id < 0)
                    {
                        //  원칙적으로 들어오면 안됨
                        empty = true;
                        break;
                    }

                    if (_newItem.id == item.id)
                    {
                        if (item.amount < maxStack)
                        {
                            empty = true;
                            break;
                        }
                    }
                }
            }
        }

        return empty;
    }

    public bool AddItem(Item _newItem)
    {
        if (IsEmpty(_newItem) == false)
        {
            Debug.Log("인벤토리가 꽉차 아이템 추가 실패");
            return false;
        }

        if (itemDB.Datas[_newItem.id].stackable)
        {
            bool isFind = false;
            int emptyIndex = -1;

            for (int i = 0; i < maxCapacity; i++)
            {
                if (items[i] == null)
                {
                    //  빈 인벤토리 인덱스 찾기
                    if (emptyIndex == -1)
                        emptyIndex = i;
                }
                else if (items[i].id == _newItem.id)
                {
                    if (items[i].amount < maxStack)
                    {
                        items[i].amount++;
                        isFind = true;
                        break;
                    }
                }
            }

            if (isFind == false)
            {
                //  같은 아이템이 없거나 있어도 스택이 풀인 경우
                if (emptyIndex == -1)
                {
                    Debug.Log("아이템 추가 인덱스 오류");
                    return false;
                }

                items[emptyIndex] = _newItem;
                itemCount++;
            }
        }
        else
        {
            int emptyIndex = -1;

            for (int i = 0; i < maxCapacity; i++)
            {
                if (items[i] == null)
                {
                    if (emptyIndex == -1)
                    {
                        emptyIndex = i;
                        break;
                    }
                }
            }

            if (emptyIndex == -1)
            {
                Debug.Log("아이템 추가 인덱스 오류");
                return false;
            }

            items[emptyIndex] = _newItem;
            itemCount++;
        }

        SoundManager.Instance.PlaySfx(SoundManager.Sfx.PickUp);
        OnItemChanged?.Invoke();

        return true;
    }

    public void RemoveItem(Item _oldItem, int _index, int _amount  = 1)
    {
        if (itemDB.Datas[_oldItem.id].stackable)
        {
            _oldItem.amount -= _amount;

            if (_oldItem.amount <= 0)
            {
                items[_index] = null;
                itemCount--;
            }
        }
        else
        {
            items[_index] = null;
            itemCount--;
        }

        OnItemChanged?.Invoke();
    }

    public void DeleteItem(int _index)
    {
        items[_index] = null;
        itemCount--;

        OnItemChanged?.Invoke();
    }

    public void SwapItems(int _indexA, int _indexB)
    {
        if (_indexA == _indexB)
            return;

        Item temp = items[_indexB];
        items[_indexB] = items[_indexA];
        items[_indexA] = temp;

        //아이템 변경시 이벤트 함수에 등록된 함수 호출
        OnItemChanged?.Invoke();
    }

    public void UseItem(Item _item, int _index)
    {

        for(int i = 0; i < _item.abilities.Length; i++)
        {
            switch (_item.abilities[i].type)
            {
                case AttributeType.Health:
                    player.Heal(_item.abilities[i].value);
                    SoundManager.Instance.PlaySfx(SoundManager.Sfx.Drink);
                    break;
                case AttributeType.Food:
                    player.CurrentHunger += _item.abilities[i].value;
                    SoundManager.Instance.PlaySfx(SoundManager.Sfx.Eat);
                    break;
            }
        }

        RemoveItem(_item, _index);

        OnItemChanged?.Invoke();
    }

    public int SearchItem(int _itemID, int _amount = 1)
    {
        if(ItemCount <= 0)
            return -1;

        for(int i = 0; i < maxCapacity; ++i)
        {
            if (Items[i] == null) continue;
            if(Items[i].id == _itemID)
            {
                if(_amount <= Items[i].amount)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    /// <summary>
    /// 장비하고 있는 아이템을 인벤토리의 아이템과 교체하는 함수
    /// </summary>
    /// <param name="_equipItem"></param>
    /// <param name="_invenIndex"></param>
    public bool SwapItemToEquip(Item _equipItem, int _invenIndex)
    {
        bool isSuccess = false;

        if (items[_invenIndex] == null)
        {
            items[_invenIndex] = _equipItem;
            itemCount++;
            isSuccess =  true;
        }
        else
        {
            if (itemDB.Datas[items[_invenIndex].id].type == itemDB.Datas[_equipItem.id].type)
            {
                Item temp = items[_invenIndex];
                items[_invenIndex] = _equipItem;
                _equipItem = null;
                Equipment.Instance.EquipItem(temp);

                isSuccess =  true;

            }
            else
            {
                Debug.Log("넣을 자리에 아이템이 있어서 아이템 추가 실패");
                isSuccess =  false;
            }
        }

        OnItemChanged?.Invoke();

        return isSuccess;
    }

    private void Update()
    {
        //  치트
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            AddItem(itemDB.Datas[3].CreateItem());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            AddItem(itemDB.Datas[4].CreateItem());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            AddItem(itemDB.Datas[41].CreateItem());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            AddItem(itemDB.Datas[9].CreateItem());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            AddItem(itemDB.Datas[10].CreateItem());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            AddItem(itemDB.Datas[14].CreateItem());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            AddItem(itemDB.Datas[8].CreateItem());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            AddItem(itemDB.Datas[26].CreateItem());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            AddItem(itemDB.Datas[48].CreateItem());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            AddItem(itemDB.Datas[54].CreateItem());
        }
    }
}
