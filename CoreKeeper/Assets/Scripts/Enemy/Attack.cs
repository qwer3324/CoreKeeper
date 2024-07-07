using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected float attackDamage = 10f;
    private bool isAttack = false;

    private void OnEnable()
    {
        isAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();

        if(character != null && !isAttack)
        {
            isAttack = true;
            character.TakeDamage(attackDamage, transform.position);
        }
    }
}
