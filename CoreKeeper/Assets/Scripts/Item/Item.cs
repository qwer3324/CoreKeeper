
[System.Serializable]
public class Item
{
    public int id = -1;
    public string name;

    //  스택 양
    public int amount;

    //  능력치
    public Ability[] abilities;

    public Item() { }

    public Item(ItemData _data)
    {
        id = _data.info.id;
        name = _data.info.name;
        amount = (_data.stackable) ? 1 : 0;

        abilities = new Ability[_data.info.abilities.Length];
        for (int i = 0; i < _data.info.abilities.Length; i++)
        {
            abilities[i] = new Ability(_data.info.abilities[i].Min, _data.info.abilities[i].Max);
            abilities[i].type = _data.info.abilities[i].type;
        }
    }
}