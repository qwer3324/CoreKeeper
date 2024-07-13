using UnityEngine;

public class Larva : Enemy
{
    protected override void StateMachineInitialize()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new LarvaState.Idle(this, stateMachine));
        stateMachine.RegisterState(new LarvaState.Chase(this, stateMachine));
        stateMachine.RegisterState(new LarvaState.Attack(this, stateMachine));

        stateMachine.ChangeState(new LarvaState.Idle());
    }

    public override void TakeDamage(float _damage, Vector3 _otherPos)
    {
        base.TakeDamage(_damage, _otherPos);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.LarvaHit);
    }
}
