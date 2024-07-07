using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    private enum PLAYER_PART { BODY, HAIR, HAIR_SHADE, EYES, TOP, BOTTOM, HOLDER }
    private SpriteRenderer[] sr;

    private static int COUNT = 39;
    private static int HOLDER_COUNT = 7;

    private Sprite[] hairSprites = new Sprite[COUNT];
    private Sprite[] helmSprites = new Sprite[COUNT];
    private Sprite[] hairShadeSprites = new Sprite[COUNT];
    private Sprite[] eyesSprites = new Sprite[COUNT];
    private Sprite[] topSprites = new Sprite[COUNT];
    private Sprite[] bottomSprites = new Sprite[COUNT];
    private Sprite[] holderSprites = new Sprite[HOLDER_COUNT];

    public HeadData head;

    private void Awake()
    {
        sr = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        hairSprites = head.Sprites;
        helmSprites = head.HelmSprites;
        hairShadeSprites = head.HairShadeSprites;
    }

    public void ChangePlayerSprite(int _num)
    {
        //   머리카락
        if (sr[(int)PLAYER_PART.HAIR].sprite != hairSprites[_num])
        {
            Debug.Log($"{sr[(int)PLAYER_PART.HAIR].sprite.name}을 {hairSprites[_num].name}으로 스프라이트 변경");
            sr[(int)PLAYER_PART.HAIR].sprite = hairSprites[_num];
            sr[(int)PLAYER_PART.HAIR_SHADE].sprite = hairShadeSprites[_num];
        }

        ////  눈
        //if (eyesSprites[_num] != null)
        //{
        //    sr[(int)PLAYER_PART.EYES].sprite = eyesSprites[_num];
        //}

        ////  상의
        //if (sr[(int)PLAYER_PART.TOP].sprite != topSprites[_num])
        //{
        //    sr[(int)PLAYER_PART.TOP].sprite = topSprites[_num];
        //}
        
        ////  하의
        //if(sr[(int)PLAYER_PART.BOTTOM].sprite != bottomSprites[_num])
        //{
        //    sr[(int)PLAYER_PART.BOTTOM].sprite = bottomSprites[_num];
        //}
    }
    
    public void ChangeHolderSprite(int _num)
    {
        if(sr[(int)PLAYER_PART.HOLDER].sprite = holderSprites[_num])
        {
            sr[(int)PLAYER_PART.HOLDER].sprite = holderSprites[_num];
        }
    }
}
