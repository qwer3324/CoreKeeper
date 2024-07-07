using UnityEngine;

[CreateAssetMenu(fileName = "Appear_", menuName = "Appearance/Data", order = 0)]
public class AppearanceData : ScriptableObject
{
    [SerializeField] private Sprite[] sprites;

    public Sprite[] Sprites { get { return sprites; } }
}
