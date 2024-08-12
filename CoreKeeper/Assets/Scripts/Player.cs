using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using System;
using MyFps.Utility;

public class Player : Character
{
    #region ����
    private SpriteRenderer[] srs;
    private Interaction interaction;
    private Transform shootPivot;
    #endregion

    #region �̵�
    private Vector2 inputMove;
    private Vector2 mouse;

    public new Direction Dir 
    {
        get { return dir; }
        set
        {
            if (dir != value)
            {
                animator.SetInteger(AnimationString.dircetion, (int)value); // ���� ����� ���� ������ �ٸ��� => ���� ������ ������
                dir = value;
                interaction.Dir = dir;
            }
        }
    }
    #endregion

    #region ����
    public enum PlayerState { Idle, Move, Attack }

    [Header("����")]
    [SerializeField] private PlayerState currentState = PlayerState.Idle;

    public void SetState(PlayerState _state)
    {
        if (currentState == _state)
            return;

        currentState = _state;

        animator.SetInteger("PlayerState", (int)currentState);
    }
    #endregion

    #region ���
    private float maxHunger = 100f;
    public float MaxHunger { get { return maxHunger; } }
    private float currentHunger = 100f;
    public float CurrentHunger 
    {
        get { return currentHunger; } 
        set 
        { 
            currentHunger = value;
            if (currentHunger > MaxHunger)
                currentHunger = MaxHunger;
                
            CharacterEvents.characterFood?.Invoke();
        } 
    }

    //  ���� {hungerTimer}���� {hungerAmount}��ŭ �پ����
    private float hungerTimer = 20f;
    private float hungerCountdown = 0f;
    private float hungerAmount = 3f;
    #endregion

    #region ���ݷ�
    //  ���� = �⺻ + ���� + ����

    [Header("���ݷ�")]
    [SerializeField, Tooltip("�⺻ ���ݷ�")] private float attackDamage = 10f;
    [SerializeField, Tooltip("���� ���ݷ�")] private float weaponDamage = 0f;
    [SerializeField, Tooltip("��� ���ݷ�")] private float equipDamage = 0f;
    [SerializeField, Tooltip("���� ���ݷ�")] private float buffDamage = 0f;
    [SerializeField, Tooltip("���� ���ݷ�"), Space(5)] private float currentAttackDamage;

    public float AttackDamage
    {
        set
        {
            attackDamage = value;
            currentAttackDamage = attackDamage + weaponDamage + equipDamage + buffDamage;
        }
    }

    public float WeaponDamage
    {
        set
        {
            weaponDamage = value;
            currentAttackDamage = attackDamage + weaponDamage + equipDamage + buffDamage;
        }
    }

    public float EquipDamage
    {
        get { return equipDamage; }
        set 
        {
            equipDamage = value;
            currentAttackDamage = attackDamage + weaponDamage + equipDamage + buffDamage;
        }
    }

    public float BuffDamage
    {
        set
        {
            buffDamage = value;
            currentAttackDamage = attackDamage + weaponDamage + equipDamage + buffDamage;
        }
    }

    public float CurrentAttackDamage { get { return currentAttackDamage; } }
    #endregion

    #region ���ݼӵ�
    [Header("���ݼӵ�")]
    [SerializeField, Tooltip("�⺻ ���� �ӵ�")] private float attackSpeed = 1;
    [SerializeField, Tooltip("���� ���� �ӵ�")] private float weaponSpeed = 0f;
    [SerializeField, Tooltip("���� ���� �ӵ�")] private float currentAttackSpeed;

    public float WeaponSpeed
    {
        set 
        {  
            weaponSpeed = value;
            currentAttackSpeed = attackSpeed + weaponSpeed;
            animator.SetFloat(AnimationString.attackSpeed, currentAttackSpeed);
        }
    }

    public float CurrentAttackSpeed { get {  return currentAttackSpeed; } }
    #endregion

    #region ġ��Ÿ
    [Header("ġ��Ÿ")]
    [SerializeField] private float criticalHitChance = 0f;
    public float CriticalHitChance { get { return criticalHitChance; } set { criticalHitChance = value; } }
    [SerializeField] private float criticalHitDamage = 1.5f;
    public float CriticalHitDamage { get { return criticalHitDamage; } set { criticalHitDamage = value; } }
    #endregion

    #region ����
    [Header("����")]
    [SerializeField] private float defencePower = 3;
    public float DefencePower { get { return defencePower; } set { defencePower = value; } }
    #endregion

    #region ȸ��
    [SerializeField] private float dodgeChance = 0f;
    public float DodgeChance { get { return dodgeChance; } set { dodgeChance = value; } }
    #endregion

    #region ���Ÿ� ���� ����
    private GameObject projectilePrefab;
    public GameObject ProjectilePrefab { set { projectilePrefab = value; } }

    private Vector2 shootDir;
    #endregion

    private float hitEffectTimer = 0.5f;
    public bool CantMove 
    { get;
      set; } = false;  //  UI ������ �� �����̰�

    public enum FootStep { Grass, Sand, Rock }
    public FootStep footStep = FootStep.Grass;

    public GameObject bloodSplattPrefab;

    protected override void Awake()
    {
        base.Awake();

        srs = GetComponentsInChildren<SpriteRenderer>();
        shootPivot = transform.Find("ShootPivot");
        interaction = GetComponentInChildren<Interaction>();
    }

    protected void Start()
    {
        currentAttackDamage = attackDamage + weaponDamage + equipDamage + buffDamage;
        currentAttackSpeed = attackSpeed + weaponSpeed;
        animator.SetFloat(AnimationString.attackSpeed, currentAttackSpeed);

        currentHealth = maxHealth;
        currentMoveSpeed = 5f;
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Move)
            rb.velocity = new Vector2(inputMove.x * currentMoveSpeed, inputMove.y * currentMoveSpeed);
        else
            rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        Hungry();
        LookAtMousePosition();

        switch (currentState)
        {
            case PlayerState.Idle:
                Idle();
                break;
            case PlayerState.Move:
                Move();
                break;
            case PlayerState.Attack:
                Attack();
                break;
            default:
                Debug.Log("�߸��� ������ �÷��̾�");
                break;
        }
    }

    private void Idle()
    {
        if (inputMove != Vector2.zero) SetState(PlayerState.Move);
    }

    private void Move()
    {
        if (inputMove == Vector2.zero) SetState(PlayerState.Idle);
    }

    private void Attack()
    {
    }

    private void Hungry()
    {
        hungerCountdown += Time.deltaTime;

        if(hungerCountdown > hungerTimer)
        {

            CurrentHunger -= hungerAmount;

            if (CurrentHunger < 0)
            {
                CurrentHunger = 0;

                currentHealth -= 5f;
                CharacterEvents.playerDamaged?.Invoke(gameObject, 5f);
            }

             hungerCountdown = 0;
        }
    }

    #region Damageable
    public override void TakeDamage(float _damage, Vector3 _otherPos)
    {
        if (IsDie) return;   //  �׾����� ����

        float dodge = UnityEngine.Random.Range(0f, 100f);

        //  ȸ�� �� ���� ��
        if (dodge >= dodgeChance)
        {
            //  ������ �޴� ����
            int finalDamage = (int)(_damage - (defencePower / 5) <= 0 ? 0 : _damage - (defencePower / 50));

            currentHealth -= finalDamage;

            //  @�׾����� üũ �� �׾��� �� �ؾ��� �� �ϱ�(ĳ���� �״� ���, �ٽ��ϱ� ȭ��)

            //  �˹�
            Vector2 dir = (transform.position - _otherPos).normalized;
            //transform.Translate(dir * new Vector2(0.5f, 0.5f));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.5f, LayerMask.GetMask("Terrian"));

            if (hit.collider != null)
            {
                transform.position = hit.point;
            }
            else
            {
                transform.Translate(dir * 50f * Time.deltaTime);
            }

            CharacterEvents.playerDamaged?.Invoke(gameObject, finalDamage);

            StartCoroutine(HitEffect());
        }
        else
        {
            CharacterEvents.playerDamaged?.Invoke(gameObject, -1);
        }

        if(currentHealth <= 0) 
        {
            Die();
        }
    }

    public new bool Heal(float _amount)
    {
        if (MaxHealth <= CurrentHealth)
            return false;

        float healAmount = CurrentHealth + _amount;

        if (MaxHealth <= healAmount)
        {
            currentHealth = MaxHealth;
            CharacterEvents.characterHeal?.Invoke(gameObject, MaxHealth - healAmount - _amount);
        }
        else
        {
            currentHealth = healAmount;
            CharacterEvents.characterHeal?.Invoke(gameObject, _amount);
        }

        return true;
    }

    public override void Die()
    {
        isDie = true;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.PlayerDie);
        animator.SetBool(AnimationString.isDie, true);
        Instantiate(bloodSplattPrefab, transform.position, Quaternion.identity);
        SceneFader.Instance.FadeOut();
    }
    #endregion

    #region Range Attack Animation Function
    public void SetShootPoint()
    {
        //  ���Ÿ� ������ �� ��ġ ����ֱ�
        shootDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPivot.position;
    }

    public void Shoot()
    {
        //  �߻�ü ���
        GameObject projectile = Instantiate(projectilePrefab, shootPivot.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetProjectile(shootDir, currentAttackDamage);
    }
    #endregion

    #region Input Event Function
    public void OnMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
    }

    public void OnAct(InputAction.CallbackContext context)
    {
        if (CantMove)
            return;

        if (context.started)
        {

            //  *Holder�� ����ִ� �������� �Ӽ��� ���� �ٸ� ���·� �����ؾ��Ѵ�*
            SetState(PlayerState.Attack);
        }
    }
    #endregion

    private void LookAtMousePosition()  //  ���콺 �ٶ󺸱�
    {
        if (CantMove)
            return;

        mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //  ĳ���Ͱ� �ٶ󺸴� ���� ����
        if (mouse.y > 0.5 && Math.Abs(mouse.y - 0.5) > Math.Abs(mouse.x - 0.5))
            Dir = Direction.Back;
        else if (mouse.y < 0.5 && Math.Abs(mouse.y - 0.5) > Math.Abs(mouse.x - 0.5))
            Dir = Direction.Front;
        else if (mouse.x > 0.5)         //  ������
            Dir = Direction.Right;
        else if (mouse.x < 0.5)         // ����
            Dir = Direction.Left;
    }

    IEnumerator HitEffect()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.PlayerHit);
        hitEffectTimer = 0.5f;

        while (hitEffectTimer >= 0f)
        {
            hitEffectTimer -= Time.deltaTime;

            for(int i = 0; i < srs.Length - 1; i++)
            {
                if (srs[i].gameObject.tag == "Shadow")
                    continue;

                srs[i].color = new Color(1f, 1f - (hitEffectTimer * 2f), 1f - (hitEffectTimer * 2f));
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (SpriteRenderer sr in srs)
        {
            if (sr.gameObject.tag == "Shadow")
                continue;

            sr.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();

        if (character != null)
        {
            float critical = UnityEngine.Random.Range(0f, 100f);
            
            //  ũ��Ƽ�� ���� ��
            if(critical < criticalHitChance)
            {
                character.TakeDamage(CurrentAttackDamage * criticalHitDamage, Vector2.right);
            }
            else
            {
                character.TakeDamage(CurrentAttackDamage, Vector2.zero);
            }
        }
    }

    public void PlayFootSound()
    {
        switch (footStep)
        {
            case FootStep.Grass:
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.GrassFootStep);
                break;
            case FootStep.Sand:
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.SeaFootStep);
                break;
            case FootStep.Rock:
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.MoldDungeonFootStep);
                break;
        }
        
    }

    public void Init()
    {
        transform.position = Vector3.zero;
        currentHealth = MaxHealth;
        currentHunger = MaxHunger;
        animator.SetBool(AnimationString.isDie, false);
        isDie = false;
        CharacterEvents.characterHeal?.Invoke(gameObject, MaxHealth);
        CharacterEvents.characterFood?.Invoke();
        SetState(PlayerState.Idle);
    }
}
