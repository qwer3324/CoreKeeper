using System.Collections.Generic;

public sealed class StateMachine
{
    private Enemy owner;

    private State currentState;
    public State CurrentState { get { return currentState; } }

    private State prevState;
    public State PrevState { get { return prevState; } }

    private float elapseTime = 0f;  //  현재 상태에 진행된 누적 시간 카운팅
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

    /// <summary>State를 StateMachine에 등록하는 함수</summary>
    public void RegisterState(State _state)
    {
        _state.OnInitialize();
        states[_state.GetType()] = _state;
    }

    public State ChangeState(State newState)
    {
        //현재 상태 체크
        var newType = newState.GetType();
        if (newType == CurrentState?.GetType())
        {
            return CurrentState;
        }

        //현재 상태에서 빠져 나오기
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }

        //현재 상태를 새로운 상태로 셋팅
        prevState = CurrentState;
        currentState = states[newType];

        //상태 들어가기
        CurrentState.OnEnter();
        elapseTime = 0f;

        return CurrentState;
    }
}
