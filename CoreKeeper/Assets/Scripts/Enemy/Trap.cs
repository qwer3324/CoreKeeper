using UnityEngine;

public class Trap : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float trapDamage = 10f;

    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float lifeCountdown = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        lifeCountdown += Time.deltaTime;

        if (lifeCountdown > lifeTime)
        {
            animator.SetBool(AnimationString.isDie, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();

        if (character != null)
        {
            character.TakeDamage(trapDamage, transform.position);
            animator.SetBool(AnimationString.isDie, true);
        }
    }
}
