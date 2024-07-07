using UnityEngine;

[CreateAssetMenu(fileName = "Appear_", menuName = "Appearance/HeadData", order = 1)]
public class HeadData : AppearanceData
{
    [SerializeField] private Sprite[] helmSprites;

    [SerializeField, Space(10)] private bool hasShade;
    [SerializeField] private Sprite[] hairShadeSprites;

    [SerializeField, Space(10)] private bool hideEyes;

    public Sprite[] HelmSprites
    {
        get { return helmSprites; }
    }

    public bool HasShade { get { return hasShade; } }

    public Sprite[] HairShadeSprites
    {
        get
        {
            if (HasShade)
                return hairShadeSprites;
            else
                return null;
        }
    }

    public bool HideEyes { get { return hideEyes; } }
}
