using UnityEngine;

namespace ShamanState
{
    public class Idle : State
    {
        private Animator animator;
        private float detectionRange = 50f;

        private float idleTimer = 1f;
        private float idleCountdown = 0f;

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
            animator.SetBool("IsDone", false);
            animator.SetInteger("State", 0);
            idleCountdown = 0f;
        }

        public override void OnUpdate(float deltaTime)
        {
            idleCountdown += deltaTime;

            if(idleCountdown > idleTimer)
            {
                if (detectionRange > (owner.Target.transform.position - owner.transform.position).sqrMagnitude)
                {
                    int random = Random.Range(1, 6);

                    if (random == 1 || random == 2)
                    {
                        stateMachine.ChangeState(new Burn());
                    }
                    else if (random == 3 || random == 4)
                    {
                        stateMachine.ChangeState(new Attack());
                    }
                    else
                    {
                        stateMachine.ChangeState(new Warp());
                    }
                    return;
                }
            }
        }
    }

    public class Burn : State
    {
        private Animator animator;
        private Vector2 targetPos;
        private float burnRange = 3f;
        private Shaman shaman;
        private float burnTimer = 0.5f;
        private float burnCountdown = 0f;
        private bool isBurn = false;

        public Burn() { }
        public Burn(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            base.OnInitialize();

            animator = owner.GetComponent<Animator>();
            shaman = owner.GetComponent<Shaman>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            targetPos = (Vector2)owner.Target.transform.position + Random.insideUnitCircle * burnRange;
            owner.SetDirection(targetPos);
            animator.SetInteger("State", 1);
            burnCountdown = 0f;
            isBurn = false;
            animator.SetBool("IsDone", false);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (isBurn)
            {
                if (animator.GetBool("IsDone"))
                {
                    stateMachine.ChangeState(new Idle());
                    owner.SetDirection(targetPos);
                    return;
                }
            }
            else
            {
                burnCountdown += deltaTime;

                if (burnCountdown > burnTimer)
                {
                    shaman.CreateFireTrap(targetPos);
                    SoundManager.Instance.PlaySfx(SoundManager.Sfx.FireWhoosh);
                    isBurn = true;
                }
            }
        }
    }

    public class Attack : State
    {
        private Animator animator;


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

            owner.SetDirection(owner.Target.transform.position);
            animator.SetBool("IsDone", false);
            animator.SetInteger("State", 2);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (animator.GetBool("IsDone"))
            {
                stateMachine.ChangeState(new Idle());
                owner.SetDirection(owner.Target.transform.position);
                return;
            }
        }
    }

    public class Warp : State
    {
        private Animator animator;
        private SpriteRenderer[] srs = new SpriteRenderer[3];

        private float warpTimer = 0.5f;
        private float warpCountdown = 0f;
        private bool isWarp = false;
        private Vector2 targetPos;


        public Warp() { }
        public Warp(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            base.OnInitialize();

            animator = owner.GetComponent<Animator>();
            
            srs = owner.GetComponentsInChildren<SpriteRenderer>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            animator.SetBool("IsDone", false);
            animator.SetInteger("State", 3);

            for(int i = 0; i < 3; i++) 
            {
                srs[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }

            warpCountdown = 0f;

            targetPos = (Vector2)owner.Target.transform.position + Random.insideUnitCircle * 7f;
            isWarp = false;

        }

        public override void OnUpdate(float deltaTime)
        {
            //if ((owner.Target.transform.position - owner.transform.position).sqrMagnitude > detectionRange)
            //{
            //    stateMachine.ChangeState(new Idle());
            //    return;
            //}

            warpCountdown += deltaTime;

            if (isWarp)
            {
                if (warpCountdown > warpTimer)
                {
                    stateMachine.ChangeState(new Idle());
                    return;
                }
            }
            else
            {
                if (warpCountdown > warpTimer / 2f)
                {
                    owner.transform.position = targetPos;
                    isWarp = true;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            for (int i = 0; i < 3; i++)
            {
                srs[i].maskInteraction = SpriteMaskInteraction.None;
            }
        }
    }
}

namespace ShamanMeleeState
{
    public class Transform : State
    {
        private Animator animator;

        private float transformTimer = 2.5f;
        private float transformCountdown = 0f;

        public Transform() { }
        public Transform(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            base.OnInitialize();

            animator = owner.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            owner.StopMoving();
        }

        public override void OnUpdate(float deltaTime)
        {
            transformCountdown += deltaTime;

            if(transformCountdown > transformTimer)
            {
                stateMachine.ChangeState(new Idle());
            }
        }
    }

    public class Idle : State
    {
        private Animator animator;

        private float idleTimer = 0.5f;
        private float idleCountdown = 0f;
        private float detectionRange = 30f;
        private float attackRange = 3f;

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
            idleCountdown = 0f;
        }

        public override void OnUpdate(float deltaTime)
        {
            idleCountdown += deltaTime;

            if (idleCountdown > idleTimer)
            {
                if (attackRange > ((owner.Target.transform.position - owner.transform.position).sqrMagnitude))
                {
                    stateMachine.ChangeState(new Attack());
                    return;
                }
                else if (detectionRange > (owner.Target.transform.position - owner.transform.position).sqrMagnitude)
                {
                    stateMachine.ChangeState(new Chase());
                    return;
                }
            }
        }
    }

    public class Chase : State
    {
        private Animator animator;

        private float attackRange = 3f;

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
            if (attackRange > ((owner.Target.transform.position - owner.transform.position).sqrMagnitude))
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
        private float attackTimer = 0.8f;
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
                stateMachine.ChangeState(new Idle());
                return;
            }
            else if (attackCountdown > 0.7f)
            {
                return;
            }
            else if (attackCountdown > 0.25f)
            {
                owner.transform.Translate(((Vector3)targetPos - owner.transform.position).normalized * Mathf.Lerp(10, 0, attackCountdown) * deltaTime);
            }
        }
    }
}