using UnityEngine;

/// <summary>
/// ���� : Rigidbody, Animator
/// / ���� : �̵��ӵ�, �̵�����
/// </summary>
public abstract class Character : MonoBehaviour
{
    #region ����
    protected Rigidbody2D rb;
    protected Animator animator;
    #endregion

    #region �̵�
    [Header("�̵��ӵ�"),SerializeField] protected float currentMoveSpeed = 100f;
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
                animator.SetInteger(AnimationString.dircetion, (int)value); // ���� ����� ���� ������ �ٸ��� => ���� ������ ������
                dir = value;
            }
        }
    }
    #endregion

    #region ü��
    [Header("ü��"), SerializeField] protected float maxHealth = 100f;
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
