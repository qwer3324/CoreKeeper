using System.Collections;
using UnityEngine;

public abstract class Enemy : Character
{
    #region 참조
    protected SpriteRenderer sr;
    #endregion

    public GameObject Target { get; private set; } //  플레이어

    private float hitEffectTimer;

    protected StateMachine stateMachine;

    [SerializeField] protected ItemData[] dropItems;
    [SerializeField] protected int[] weights;
    protected WeightedRandomPicker<int> wrp = new WeightedRandomPicker<int>();
    
    protected override void Awake()
    {
        base.Awake();

        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        Target = GameObject.FindGameObjectWithTag("Player");
        StateMachineInitialize();

        //  드랍 아이템 가중치 삽입
        for (int i = 0; i < dropItems.Length; i++)
        {
            wrp.Add(i, weights[i]);
        }
    }

    protected virtual void Update()
    {
        if(!IsDie)
            stateMachine.Update(Time.deltaTime);
    }

    //  StateMachine 초기화
    protected abstract void StateMachineInitialize();

    public Vector2 Move(Vector2 _targetPos, float _moveSpeed)
    {
        Vector2 pos = IsMoveablePosition(_targetPos);
        Vector2 targetDir = (pos - (Vector2)transform.position).normalized;

        rb.velocity = targetDir * _moveSpeed * Time.deltaTime;

        return pos;
    }

    public Vector2 IsMoveablePosition(Vector2 _targetPos)
    {
        Vector2 targetDir = (_targetPos - (Vector2)transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir, _targetPos.magnitude, LayerMask.GetMask("Terrian", "Water"));

        if (hit.collider != null)
        {
            return hit.point;
        }
        else
        {
            return _targetPos;
        }
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    public void SetDirection(Vector2 _movePoint)
    {
        if (Mathf.Abs(transform.position.x - _movePoint.x) > Mathf.Abs(transform.position.y - _movePoint.y))
        {
            //  왼쪽
            if (_movePoint.x < transform.position.x)
            {
                Dir = Direction.Left;
            }
            else
            {
                Dir = Direction.Right;
            }
        }
        else
        {
            if (_movePoint.y < transform.position.y)
            {
                Dir = Direction.Front;
            }
            else
            {
                Dir = Direction.Back;
            }
        }
    }


    public State ChangeState(State newState)
    {
        return stateMachine.ChangeState(newState);
    }

    IEnumerator HitEffect()
    {
        hitEffectTimer = 0.5f;

        while (hitEffectTimer >= 0f)
        {
            hitEffectTimer -= Time.deltaTime;
            sr.color = new Color(1f, 1f - (hitEffectTimer * 2f), 1f - (hitEffectTimer * 2f));
            yield return new WaitForEndOfFrame();
        }
        sr.color = Color.white;
    }

    #region Damageable
    public override void TakeDamage(float _damage, Vector3 _otherPos)
    {
        if (IsDie)
            return;

        currentHealth -= _damage;

        //  otherPos로 들어온 값으로 크리티컬인지 아닌지 구분한다
        //  Vector2.zero = 노 크리티컬, Vector2.right = 크리티컬

        if(_otherPos.magnitude > 0f) 
        {
            CharacterEvents.enemyDamaged?.Invoke(gameObject, -_damage);
        }
        else
        {
            CharacterEvents.enemyDamaged?.Invoke(gameObject, _damage);
        }
        

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(HitEffect());
    }

    public override void Die()
    {
        if (rb != null)
            rb.velocity = Vector2.zero;
        animator.SetBool(AnimationString.isDie, true);
        isDie = true;

        //  아이템 드랍
        if (dropItems.Length > 0)
        {
            int index = wrp.GetRandomPick();

            GameObject obj = Instantiate(Inventory.Instance.dropItemPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<DropItem>().SetItemData(dropItems[index]);
        }

        Destroy(gameObject, 2f);
    }
    #endregion
}
