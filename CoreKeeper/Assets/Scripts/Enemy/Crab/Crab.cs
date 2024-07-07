using System.Collections;
using UnityEngine;

public class Crab : Enemy
{
    public GameObject projectilePrefab;
    public float attackDamage = 40;

    private int bubbleAmount = 5;

    protected override void StateMachineInitialize()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new CrabState.Idle(this, stateMachine));
        stateMachine.RegisterState(new CrabState.Chase(this, stateMachine));
        stateMachine.RegisterState(new CrabState.Attack(this, stateMachine));

        stateMachine.ChangeState(new CrabState.Idle());
    }

    public void CreateProjectiles()
    {
        StartCoroutine(CreateProjectile());
    }

    IEnumerator CreateProjectile()
    {
        for(int i = 0; i < bubbleAmount; i++) 
        {
            Vector2 shootDir = (Target.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Bubble>().dir = shootDir;
            projectile.GetComponent<Bubble>().attackDamage = attackDamage;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
