public enum ItemType : int
{
    Helm = 0,
    Chest = 1,
    Pants = 2,
    Necklace = 3,
    Ring = 4,
    Weapon = 5, //  장비템은 이 위에 넣기
    Food = 6,
    Potion = 7, //  소비템은 이 위에 넣기
    Placement = 8,
    CraftingStation = 9,
    Key = 10,
    CraftingMaterial = 11,
    Miscellaneous
}

public enum AttributeType
{
    MaxHealth,
    Armor,
    DodgeChance,
    CriticalHitDamage,
    MeleeDamage,
    RangeDamage,
    Health,
    CriticalHitChance,
    Food,
}