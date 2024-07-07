using UnityEngine;

[CreateAssetMenu(fileName = "CraftData", menuName = "Craft/Craft Data")]

public class CraftData : ScriptableObject
{
    public int itemID;
    public int[] materialsID;
    public int[] materialsAmount;
}