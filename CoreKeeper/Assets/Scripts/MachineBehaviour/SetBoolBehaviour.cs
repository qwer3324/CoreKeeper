using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour
{
    public string boolName;
    public bool updateOnState;
    public bool updateOnStateMachine;
    public bool valueEnter, valueExit;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState) animator.SetBool(boolName, valueEnter);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState) animator.SetBool(boolName, valueExit);
    }

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine) animator.SetBool(boolName, valueEnter);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine) animator.SetBool(boolName, valueExit);
    }
}
