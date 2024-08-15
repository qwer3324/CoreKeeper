using UnityEngine;

public class MoldTentacle : Enemy
{
    public GameObject projectilePrefab;
    public float attackDamage = 10;
    public Vector2 offset;

    protected override void StateMachineInitialize()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new MoldTentacleState.Idle(this, stateMachine));
        stateMachine.RegisterState(new MoldTentacleState.Attack(this, stateMachine));

        stateMachine.ChangeState(new MoldTentacleState.Idle());
    }

    public void CreateProjectile()
    {
        Vector2 shootDir = (Target.transform.position - (transform.position + (Vector3)offset));
        GameObject projectile = Instantiate(projectilePrefab, transform.position + (Vector3)offset, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetProjectile(shootDir, attackDamage, this);
    }

    public override void TakeDamage(float _damage, Vector3 _otherPos)
    {
        base.TakeDamage(_damage, _otherPos);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Hit);
    }
}
