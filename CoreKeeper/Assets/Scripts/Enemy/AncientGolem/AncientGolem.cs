using UnityEngine;

public class AncientGolem : Enemy
{
    public GameObject effectPrefab;
    public Transform effectPivot;

    protected override void StateMachineInitialize()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new AncientGolemState.Idle(this, stateMachine));
        stateMachine.RegisterState(new AncientGolemState.Chase(this, stateMachine));
        stateMachine.RegisterState(new AncientGolemState.Attack(this, stateMachine));

        stateMachine.ChangeState(new AncientGolemState.Idle());
    }

    public void CreateEffect()
    {
        GameObject obj = Instantiate(effectPrefab, effectPivot.position, Quaternion.identity);
    }
}