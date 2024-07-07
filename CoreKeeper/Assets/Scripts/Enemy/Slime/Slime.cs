using UnityEngine;

public class Slime : Enemy
{
    protected override void StateMachineInitialize()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new SlimeState.Idle(this, stateMachine));
        stateMachine.RegisterState(new SlimeState.Chase(this, stateMachine));
        stateMachine.RegisterState(new SlimeState.Attack(this, stateMachine));

        stateMachine.ChangeState(new SlimeState.Idle());
    }

    public override void TakeDamage(float _damage, Vector3 _otherPos)
    {
        base.TakeDamage(_damage, _otherPos);

        if(stateMachine.CurrentState.GetType() == new SlimeState.Idle().GetType())
            stateMachine.ChangeState(new SlimeState.Chase());
    }
}
