using UnityEngine;

/// <summary>
/// 참조 : Rigidbody, Animator
/// / 변수 : 이동속도, 이동방향
/// </summary>
public abstract class Character : MonoBehaviour
{
    #region 참조
    protected Rigidbody2D rb;
    protected Animator animator;
    #endregion

    #region 이동
    [Header("이동속도"),SerializeField] protected float currentMoveSpeed = 100f;
    public float CurrentMoveSpeed { get { return currentMoveSpeed; } }

    public enum Direction { Front, Back, Left, Right}
    protected Direction dir = Direction.Front;
    public Direction Dir
    {
        get { return dir; }
        set
        {
            if (dir != value)
            {
                animator.SetInteger(AnimationString.dircetion, (int)value); // 현재 방향과 들어온 방향이 다르면 => 들어온 방향을 봐야함
                dir = value;
            }
        }
    }
    #endregion

    #region 체력
    [Header("체력"), SerializeField] protected float maxHealth = 100f;
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    [SerializeField] protected float currentHealth = 100f;
    public float CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    protected bool isDie = false;
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    #endregion

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void PlaySound(SoundManager.Sfx sfx)
    {
        SoundManager.Instance.PlaySfx(sfx);
    }

    #region Damageable
    public bool Heal(float _amount)
    {
        if (MaxHealth <= CurrentHealth)
            return false;

        float healAmount = CurrentHealth + _amount;

        if (MaxHealth <= healAmount)
            currentHealth = MaxHealth;
        else
            currentHealth = healAmount;

        return true;
    }

    public abstract void TakeDamage(float _damage, Vector3 _otherPos);

    public abstract void Die();
    #endregion
}
