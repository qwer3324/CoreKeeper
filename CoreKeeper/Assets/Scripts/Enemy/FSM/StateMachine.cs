using System.Collections.Generic;

public sealed class StateMachine
{
    private Enemy owner;

    private State currentState;
    public State CurrentState { get { return currentState; } }

    private State prevState;
    public State PrevState { get { return prevState; } }

    private float elapseTime = 0f;  //  ���� ���¿� ����� ���� �ð� ī����
    public float ElapseTime { get {  return elapseTime; } }

    private Dictionary<System.Type, State> states = new Dictionary<System.Type, State>();

    public StateMachine(Enemy _enemy)
    {
        owner = _enemy;
    }

    public void Update(float deltaTime)
    {
        elapseTime += deltaTime;
        CurrentState.OnUpdate(deltaTime);
    }

    /// <summary>State�� StateMachine�� ����ϴ� �Լ�</summary>
    public void RegisterState(State _state)
    {
        _state.OnInitialize();
        states[_state.GetType()] = _state;
    }

    public State ChangeState(State newState)
    {
        //���� ���� üũ
        var newType = newState.GetType();
        if (newType == CurrentState?.GetType())
        {
            return CurrentState;
        }

        //���� ���¿��� ���� ������
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }

        //���� ���¸� ���ο� ���·� ����
        prevState = CurrentState;
        currentState = states[newType];

        //���� ����
        CurrentState.OnEnter();
        elapseTime = 0f;

        return CurrentState;
    }
}
