using UnityEngine;

namespace AncientGolemState
{
    public class Idle : State
    {
        private Animator animator;
        private float detectionRange = 30f;

        public Idle() { }
        public Idle(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            base.OnInitialize();

            animator = owner.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            animator.SetInteger("State", 0);
            owner.StopMoving();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (detectionRange > (owner.Target.transform.position - owner.transform.position).sqrMagnitude)
            {
                stateMachine.ChangeState(new Chase());
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            animator.SetInteger("State", 3);
        }
    }

    public class Chase : State
    {
        private Animator animator;

        private float detectionRange = 30f;
        private float attackRange = 2.5f;

        public Chase() { }
        public Chase(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            base.OnInitialize();

            animator = owner.GetComponent<Animator>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if(animator.GetInteger("State") == 1)
            {
                if (detectionRange < ((owner.Target.transform.position - owner.transform.position).sqrMagnitude))
                {
                    stateMachine.ChangeState(new Idle());
                    return;
                }
                else if (attackRange > ((owner.Target.transform.position - owner.transform.position).sqrMagnitude))
                {
                    stateMachine.ChangeState(new Attack());
                    return;
                }

                owner.SetDirection(owner.Target.transform.position);
                owner.Move(owner.Target.transform.position, owner.CurrentMoveSpeed);
            }
        }
    }

    public class Attack : State
    {
        private Animator animator;
        private float attackTimer = 1.5f;
        private float attackCountdown = 0f;
        private Vector2 targetPos = Vector2.zero;

        public Attack() { }

        public Attack(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            base.OnInitialize();

            animator = owner.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            animator.SetTrigger(AnimationString.attackTrigger);
            animator.SetInteger("State", 2);
            attackCountdown = 0f;
            owner.StopMoving();
            targetPos = owner.Target.transform.position;
            owner.SetDirection(targetPos);
        }

        public override void OnUpdate(float deltaTime)
        {
            attackCountdown += deltaTime;

            if (attackCountdown > attackTimer)
            {
                stateMachine.ChangeState(new Chase());
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            animator.SetInteger("State", 1);
        }
    }
}
