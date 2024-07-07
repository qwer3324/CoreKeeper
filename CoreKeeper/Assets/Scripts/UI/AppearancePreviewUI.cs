using UnityEngine;
using UnityEngine.UI;

public class AppearancePreviewUI : MonoBehaviour
{
    private Appearance appearance;
    private Image[] images;
    [SerializeField] private Sprite[] defaultSprite;

    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
        appearance = GameObject.FindGameObjectWithTag("Player").GetComponent<Appearance>();

        defaultSprite = new Sprite[images.Length];

        defaultSprite[(int)Appearance.PlayerPart.Body] = appearance.gender.BodySprites[0];
        defaultSprite[(int)Appearance.PlayerPart.Head] = appearance.head.Sprites[0];
        defaultSprite[(int)Appearance.PlayerPart.HairShade] = appearance.head.HairShadeSprites[0];
        defaultSprite[(int)Appearance.PlayerPart.Eyes] = appearance.gender.EyesSprites[0];
        defaultSprite[(int)Appearance.PlayerPart.Top] = appearance.top.Sprites[0];
        defaultSprite[(int)Appearance.PlayerPart.Bottom] = appearance.bottom.Sprites[0];

        Equipment.Instance.OnEquipmentChanged += Refresh;
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (images[(int)Appearance.PlayerPart.Body].sprite != appearance.gender.BodySprites[0])
        {
            images[(int)Appearance.PlayerPart.Body].sprite = appearance.gender.BodySprites[0];
        }

        if (images[(int)Appearance.PlayerPart.Head].sprite != appearance.head.Sprites[0])
        {
            images[(int)Appearance.PlayerPart.Head].sprite = appearance.head.Sprites[0];

            if (appearance.head.HasShade)
            {
                images[(int)Appearance.PlayerPart.HairShade].gameObject.SetActive(true);
                images[(int)Appearance.PlayerPart.HairShade].sprite = appearance.head.HairShadeSprites[0];
            }
            else
            {
                images[(int)Appearance.PlayerPart.HairShade].gameObject.SetActive(false);
            }
        }

        if (appearance.head.HideEyes)
        {
            images[(int)Appearance.PlayerPart.Eyes].gameObject.SetActive(false);
        }
        else
        {
            images[(int)Appearance.PlayerPart.Eyes].gameObject.SetActive(true);
            if (images[(int)Appearance.PlayerPart.Eyes].sprite != appearance.gender.EyesSprites[0])
            {
                images[(int)Appearance.PlayerPart.Eyes].sprite = appearance.gender.EyesSprites[0];
            }
        }

        if (images[(int)Appearance.PlayerPart.Top].sprite != appearance.top.Sprites[0])
        {
            if (appearance.top.Sprites[0] == null)
            {
                images[(int)Appearance.PlayerPart.Top].color = Color.clear;
            }
            else
            {
                images[(int)Appearance.PlayerPart.Top].sprite = appearance.top.Sprites[0];
            }
        }

        if (images[(int)Appearance.PlayerPart.Bottom].sprite != appearance.bottom.Sprites[0])
        {
            if (appearance.bottom.Sprites[0] == null)
            {
                images[(int)Appearance.PlayerPart.Bottom].color = Color.clear;
            }
            else
            {
                images[(int)Appearance.PlayerPart.Bottom].sprite = appearance.bottom.Sprites[0];
            }
        }
    }
}
