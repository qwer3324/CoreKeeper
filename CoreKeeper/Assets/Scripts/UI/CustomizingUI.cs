using UnityEngine;
using UnityEngine.UI;

public class CustomizingUI : MonoBehaviour
{
    private Appearance appearance;
    private Image[] images;

    public GenderData[] genderDatas;
    public HeadData[] headDatas;

    private int genderIndex = 0;
    private int headIndex = 0;

    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
        appearance = GameObject.FindGameObjectWithTag("Player").GetComponent<Appearance>();

    }

    private void Start()
    {
        images[(int)Appearance.PlayerPart.Body].sprite = genderDatas[genderIndex].BodySprites[0];
        images[(int)Appearance.PlayerPart.Head].sprite = headDatas[headIndex].Sprites[0];
        images[(int)Appearance.PlayerPart.HairShade].sprite = headDatas[headIndex].HairShadeSprites[0];
        images[(int)Appearance.PlayerPart.Eyes].sprite = genderDatas[genderIndex].EyesSprites[0];
    }

    public void GenderRightButton()
    {
        genderIndex++;

        if(genderIndex >= genderDatas.Length) 
        {
            genderIndex = 0;
        }

        images[(int)Appearance.PlayerPart.Body].sprite = genderDatas[genderIndex].BodySprites[0];
        images[(int)Appearance.PlayerPart.Eyes].sprite = genderDatas[genderIndex].EyesSprites[0];
    }

    public void GenderLeftButton()
    {
        genderIndex--;

        if (genderIndex < 0)
        {
            genderIndex = genderDatas.Length - 1;
        }

        images[(int)Appearance.PlayerPart.Body].sprite = genderDatas[genderIndex].BodySprites[0];
        images[(int)Appearance.PlayerPart.Eyes].sprite = genderDatas[genderIndex].EyesSprites[0];
    }

    public void HeadRightButton()
    {
        headIndex++;

        if (headIndex >= headDatas.Length)
        {
            headIndex = 0;
        }

        images[(int)Appearance.PlayerPart.Head].sprite = headDatas[headIndex].Sprites[0];
        if (headDatas[headIndex].HasShade)
            images[(int)Appearance.PlayerPart.HairShade].sprite = headDatas[headIndex].HairShadeSprites[0];
    }

    public void HeadLeftButton()
    {
        headIndex--;

        if (headIndex < 0)
        {
            headIndex = headDatas.Length - 1;
        }

        images[(int)Appearance.PlayerPart.Head].sprite = headDatas[headIndex].Sprites[0];
        if (headDatas[headIndex].HasShade)
            images[(int)Appearance.PlayerPart.HairShade].sprite = headDatas[headIndex].HairShadeSprites[0];
    }

    public void ChangeButton()
    {
        appearance.SetGenderData(genderDatas[genderIndex]);
        appearance.SetAppearanceData(Appearance.PlayerPart.Head, headDatas[headIndex]);
        UIManager.Instance.ChangeUiMode(UIManager.UI_Mode.Normal);
    }
}