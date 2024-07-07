public abstract class State
{
    protected StateMachine stateMachine;
    protected Enemy owner;

    protected State() { }
    protected State(Enemy _enemy, StateMachine _stateMachine)
    {
        owner = _enemy;
        stateMachine = _stateMachine;
    }

    public virtual void OnInitialize() { }

    public virtual void OnEnter() { }

    public abstract void OnUpdate(float deltaTime); // 구현 강제

    public virtual void OnExit() { }
}
