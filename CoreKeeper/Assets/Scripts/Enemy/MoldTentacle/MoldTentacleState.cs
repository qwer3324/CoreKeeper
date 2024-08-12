using UnityEngine;

namespace MoldTentacleState
{
    public class Idle : State
    {
        private float detectionDist = 30f;

        public Idle() { }
        public Idle(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnUpdate(float deltaTime)
        {
            if ((owner.Target.transform.position - owner.transform.position).sqrMagnitude < detectionDist)
            {
                stateMachine.ChangeState(new Attack());
            }
        }
    }

    public class Attack : State
    {
        private Animator animator;

        private float detectionDist = 30f;
        private float attackDelay = 2.5f;
        private float attackTimer = 0f;

        public Attack() { }
        public Attack(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            animator = owner.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            attackTimer = attackDelay;
        }

        public override void OnUpdate(float deltaTime)
        {
            if ((owner.Target.transform.position - owner.transform.position).sqrMagnitude > detectionDist)
            {
                stateMachine.ChangeState(new Idle());
                return;
            }


            Vector2 dir = (owner.Target.transform.position - owner.transform.position).normalized;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x < 0)
                {
                    owner.Dir = Character.Direction.Left;
                }
                else
                    owner.Dir = Character.Direction.Right;
            }
            else
            {
                if (dir.y < 0)
                {
                    owner.Dir = Character.Direction.Front;
                }
                else
                    owner.Dir = Character.Direction.Back;
            }

            attackTimer += deltaTime;

            if (attackDelay < attackTimer)
            {
                attackTimer = 0f;

                RaycastHit2D hit = Physics2D.Raycast(owner.transform.position, dir, 5f, LayerMask.GetMask("Terrian"));

                if (hit.collider == null)
                {
                    animator.SetTrigger(AnimationString.attackTrigger);
                }
            }
        }
    }
}

