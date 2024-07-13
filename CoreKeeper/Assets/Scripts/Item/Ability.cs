using UnityEngine;

[System.Serializable]
public class Ability
{
    public AttributeType type;
    public int value;

    [SerializeField] private int min;
    [SerializeField] private int max;

    public int Min => min;
    public int Max => max;

    public Ability(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    private void GenerateValue()
    {
        value = Random.Range(min, max);
    }
}
