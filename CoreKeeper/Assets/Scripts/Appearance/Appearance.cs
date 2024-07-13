using UnityEngine;

public class Appearance : MonoBehaviour
{
    public enum PlayerPart { Body, Head, HairShade, Eyes, Top, Bottom, Holder }
    private enum WeaponType { Melee, Range }
    private WeaponType weaponType;

    private SpriteRenderer[] sr;
    private Animator animator;
    public GameObject holderPivot;

    private int frameNumber = 0;
    private int holderNumber = 0;

    private bool hasShade;

    //  실제 보여지는 스프라이트
    private Sprite[] bodySprites;
    private Sprite[] headSprites;
    private Sprite[] helmSprites;
    private Sprite[] hairShadeSprites;
    private Sprite[] eyesSprites;
    private Sprite[] topSprites;
    private Sprite[] bottomSprites;
    private Sprite[] holderSprites;

    //  기본 스프라이트 데이터
    public GenderData defaultGender;
    public HeadData defaultHead;
    public AppearanceData defaultTop;
    public AppearanceData defaultBottom;

    //  현재 스프라이트 데이터
    [Space(10)]
    public GenderData gender;
    public HeadData head;
    public AppearanceData top;
    public AppearanceData bottom;
    public AppearanceData holder;

    private void Awake()
    {
        sr = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        bodySprites = gender.BodySprites;
        eyesSprites = gender.EyesSprites;

        headSprites = head.Sprites;
        helmSprites = head.HelmSprites;
        hasShade = head.HasShade;
        if (hasShade)
        {
            hairShadeSprites = head.HairShadeSprites;
        }

        topSprites = top.Sprites;
        bottomSprites = bottom.Sprites;

        if (holder != null)
        {
            holderSprites = holder.Sprites;

            //  스프라이트 수로 무기 타입 정의하기
            if (holderSprites.Length > 10)
            {
                weaponType = WeaponType.Range;
                animator.SetBool("IsRange", true);
            }
            else
            {
                weaponType = WeaponType.Melee;
                animator.SetBool("IsRange", false);
            }
        }
    }

    //  애니메이션 호출용
    public void ChangePlayerSprite(int _num)
    {
        frameNumber = _num;

        //  몸
        if (sr[(int)PlayerPart.Body].sprite != bodySprites[frameNumber])
        {
            sr[(int)PlayerPart.Body].sprite = bodySprites[frameNumber];
        }

        //   머리카락
        if (sr[(int)PlayerPart.Head].sprite != headSprites[frameNumber])
        {
            sr[(int)PlayerPart.Head].sprite = headSprites[frameNumber];

            if (hasShade)
                sr[(int)PlayerPart.HairShade].sprite = hairShadeSprites[frameNumber];
        }

        //  눈
        if (eyesSprites[_num] != null && sr[(int)PlayerPart.Eyes].sprite != eyesSprites[frameNumber])
        {
            sr[(int)PlayerPart.Eyes].sprite = eyesSprites[frameNumber];
        }

        //  상의
        if (sr[(int)PlayerPart.Top].sprite != topSprites[frameNumber])
        {
            sr[(int)PlayerPart.Top].sprite = topSprites[frameNumber];
        }

        //  하의
        if (sr[(int)PlayerPart.Bottom].sprite != bottomSprites[frameNumber])
        {
            sr[(int)PlayerPart.Bottom].sprite = bottomSprites[frameNumber];
        }
    }

    //  애니메이션 호출용
    public void ChangeHolderSprite(int _num)
    {
        if (holder == null)
        {
            holderNumber = 0;
            sr[(int)PlayerPart.Holder].sprite = null;
            return;
        }

        if (sr[(int)PlayerPart.Holder].sprite != holderSprites[_num])
        {
            holderNumber = _num;
            sr[(int)PlayerPart.Holder].sprite = holderSprites[_num];
        }
    }

    public void ChangeIdleHolderSprite(int _num)
    {
        if (holder == null)
        {
            holderNumber = 0;
            sr[(int)PlayerPart.Holder].sprite = null;
            return;
        }

        if(weaponType == WeaponType.Melee)
        {
            if (sr[(int)PlayerPart.Holder].sprite != holderSprites[0])
            {
                holderNumber = 0;
                sr[(int)PlayerPart.Holder].sprite = holderSprites[0];
            }
        }
        else
        {
            if (sr[(int)PlayerPart.Holder].sprite != holderSprites[_num])
            {
                holderNumber = _num;
                sr[(int)PlayerPart.Holder].sprite = holderSprites[_num];
            }
        }
    }

    private void Refresh()
    {
        //  Data => Sprite 적용
        headSprites = head.Sprites;
        if (head.HideEyes)
        {
            sr[(int)PlayerPart.Eyes].gameObject.SetActive(false);
        }
        else
        {
            sr[(int)PlayerPart.Eyes].gameObject.SetActive(true);
        }

        if (head.HasShade)
        {
            sr[(int)PlayerPart.HairShade].enabled = true;
            hairShadeSprites = head.HairShadeSprites;
        }
        else
        {
            sr[(int)PlayerPart.HairShade].enabled = false;
        }

        topSprites = top.Sprites;
        bottomSprites = bottom.Sprites;

        if (holder != null)
        {
            holderSprites = holder.Sprites;
            //  스프라이트 수로 무기 타입 정의하기
            if (holderSprites.Length > 10)
            {
                weaponType = WeaponType.Range;
                animator.SetBool("IsRange", true);
            }
            else
            {
                weaponType = WeaponType.Melee;
                animator.SetBool("IsRange", false);
            }
        }
        else
        {
            holderSprites = null;
            weaponType = WeaponType.Melee;
            animator.SetBool("IsRange", false);
        }

        switch (weaponType)
        {
            //  HolderPivot 위치 바꾸기
            case WeaponType.Melee:
                holderPivot.transform.localPosition = new Vector3(0, 0, 0);
                //sr[(int)PlayerPart.Holder].transform.position = Vector3.zero;
                break;
            case WeaponType.Range:
                //sr[(int)PlayerPart.Holder].transform.position = new Vector3(-0.313f, 0.65f , 0);
                holderPivot.transform.localPosition = new Vector3(0, 0.3f, 0);
                break;
        }

        ChangePlayerSprite(frameNumber);
        ChangeHolderSprite(holderNumber);
    }

    public void SetGenderData(GenderData _data)
    {
        if (_data == null) gender = defaultGender;
        else gender = _data;

        bodySprites = gender.BodySprites;
        eyesSprites = gender.EyesSprites;
    }

    public void SetAppearanceData(PlayerPart _part , AppearanceData _data)
    {
        switch (_part)
        {
            case PlayerPart.Head:
                if (_data == null) head = defaultHead;
                else head = _data as HeadData;
                headSprites = head.Sprites;
                break;
            case PlayerPart.Top:
                if (_data == null) top = defaultTop;
                else top = _data;
                topSprites = top.Sprites;
                break;
            case PlayerPart.Bottom:
                if (_data == null) bottom = defaultBottom;
                else bottom = _data;
                bottomSprites = bottom.Sprites;
                break;
            case PlayerPart.Holder:
                if (_data == null) holder = null;
                else holder = _data;
                if(holder != null)
                    holderSprites = holder.Sprites;
                else
                    holderSprites = null;
                break;
        }

        Refresh();
    }
}
