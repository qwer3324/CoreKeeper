using UnityEngine;

public class SetIntegerBehaviour : StateMachineBehaviour
{
    public string integerName;
    public bool updateOnState;
    public bool updateOnStateMachine;
    public int valueEnter, valueExit;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState) animator.SetInteger(integerName, valueEnter);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState) animator.SetInteger(integerName, valueExit);
    }

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine) animator.SetInteger(integerName, valueEnter);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine) animator.SetInteger(integerName, valueExit);
    }
}
