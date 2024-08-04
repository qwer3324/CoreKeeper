using UnityEngine;

namespace LarvaState
{
    public class Idle : State
    {
        private Animator animator;

        private Vector2 targetPos;
        private float idleRange = 3f;
        private bool isArrive = false;

        private float idleTimer = 1f;       //  대기 시간
        private float idleCountdown = 0f;

        private float detectionRange = 10f;

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

            //animator.SetInteger("State", 1);
            //GetRandomPointInCircle();
            //targetPos = owner.Move(targetPos, owner.CurrentMoveSpeed);
            //owner.SetDirection(targetPos);
            isArrive = true;
            idleCountdown = 0f;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (detectionRange > (owner.Target.transform.position - owner.transform.position).sqrMagnitude)
            {
                stateMachine.ChangeState(new Chase());
                return;
            }

            if (isArrive)
            {
                idleCountdown += deltaTime;

                if (idleCountdown > idleTimer)
                {
                    animator.SetInteger("State", 1);
                    GetRandomPointInCircle();
                    targetPos = owner.Move(targetPos, owner.CurrentMoveSpeed);
                    owner.SetDirection(targetPos);
                    isArrive = false;
                    idleCountdown = 0;
                }

            }
            else
            {
                if (((Vector2)owner.transform.position - targetPos).sqrMagnitude < 0.2f)
                {
                    isArrive = true;
                    animator.SetInteger("State", 0);
                    owner.StopMoving();
                }
            }
        }

        private void GetRandomPointInCircle()
        {
            targetPos = (Vector2)owner.transform.position + Random.insideUnitCircle * idleRange;
        }
    }

    public class Chase : State
    {
        private Animator animator;

        private float detectionRange = 30f;
        private float attackRange = 1f;

        public Chase() { }
        public Chase(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            base.OnInitialize();

            animator = owner.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            animator.SetInteger("State", 1);
        }

        public override void OnUpdate(float deltaTime)
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

    public class Attack : State
    {
        private Animator animator;
        private float attackTimer = 1f;
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
            else if (attackCountdown > 0.7f)
            {
                return;
            }
            else if (attackCountdown > 0.5f)
            {
                owner.transform.Translate(((Vector3)targetPos - owner.transform.position).normalized * Mathf.Lerp(10, 0, attackCountdown) * deltaTime);
            }
        }
    }
}
