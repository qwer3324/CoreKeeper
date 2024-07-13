using UnityEngine;

[CreateAssetMenu(fileName = "Gender_", menuName = "Customizing/Gender", order = 1)]
public class GenderData : ScriptableObject
{
    [SerializeField] private bool isMale;
    [SerializeField] private Sprite[] bodySprites;
    [SerializeField] private Sprite[] eyesSprites;

    public bool IsMale { get { return isMale; } }
    public Sprite[] BodySprites { get { return bodySprites; } }
    public Sprite[] EyesSprites { get { return eyesSprites; } }
}
