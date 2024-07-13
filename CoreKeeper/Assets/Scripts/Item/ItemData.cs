using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class ItemData : ScriptableObject
{
    public ItemType type;
    public bool stackable;

    public Sprite icon;
    public Sprite dropIcon;
    [TextArea(15, 20)] public string description;

    public AppearanceData appearance;

    public Item info = new Item();

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}
