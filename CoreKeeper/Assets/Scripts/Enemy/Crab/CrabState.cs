using UnityEngine;

namespace CrabState
{
    public class Idle : State
    {
        private Animator animator;

        #region Variables
        private float idleTimer = 5f;       //  대기 시간
        private float idleMaxTime = 10f;
        private float idleMinTime = 3f;
        private float idleCountdown = 0f;
        private float minRange = 3f;       //  이동 반경
        private float maxRange = 6f;

        private bool isArrive = false;      //  목표 지점에 도착했는지
        private Vector2 targetPos;          //  목표 지점

        private float detectionRange = 20f;
        #endregion

        public Idle() { }
        public Idle(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            base.OnInitialize();

            animator = owner.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            owner.Dir = (Character.Direction)Random.Range(0, 4);

            switch (owner.Dir)
            {
                case Character.Direction.Front:
                    targetPos = owner.transform.position + new Vector3(0f, Random.Range(-minRange, -maxRange));
                    break;
                case Character.Direction.Back:
                    targetPos = owner.transform.position + new Vector3(0f, Random.Range(minRange, maxRange));
                    break;
                case Character.Direction.Left:
                    targetPos = owner.transform.position + new Vector3(Random.Range(-minRange, -maxRange), 0f);
                    break;
                case Character.Direction.Right:
                    targetPos = owner.transform.position + new Vector3(Random.Range(minRange, maxRange), 0f);
                    break;
            }

            targetPos = owner.Move(targetPos, owner.CurrentMoveSpeed);

            idleTimer = Random.Range(idleMinTime, idleMaxTime);
            idleCountdown = idleTimer;
            isArrive = false;
            animator.SetInteger("State", 1);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (detectionRange > (owner.Target.transform.position - owner.transform.position).sqrMagnitude)
            {
                stateMachine.ChangeState(new Chase());
                return;
            }

            //  도착했다
            if (isArrive)
            {
                idleCountdown -= Time.deltaTime;

                owner.StopMoving();

                if (idleCountdown <= 0)
                {
                    idleTimer = Random.Range(idleMinTime, idleMaxTime);
                    idleCountdown = idleTimer;
                    isArrive = false;

                    owner.Dir = (Character.Direction)Random.Range(0, 4);

                    switch (owner.Dir)
                    {
                        case Character.Direction.Front:
                            targetPos = owner.transform.position + new Vector3(0f, Random.Range(-minRange, -maxRange));
                            break;
                        case Character.Direction.Back:
                            targetPos = owner.transform.position + new Vector3(0f, Random.Range(minRange, maxRange));
                            break;
                        case Character.Direction.Left:
                            targetPos = owner.transform.position + new Vector3(Random.Range(-minRange, -maxRange), 0f);
                            break;
                        case Character.Direction.Right:
                            targetPos = owner.transform.position + new Vector3(Random.Range(minRange, maxRange), 0f);
                            break;
                    }

                    targetPos = owner.Move(targetPos, owner.CurrentMoveSpeed);
                    animator.SetInteger("State", 1);
                }
            }
            //  목표까지 움직이는 중
            else
            {
                if (((Vector2)owner.transform.position - targetPos).sqrMagnitude < 0.5f)
                {
                    isArrive = true;
                    animator.SetInteger("State", 0);
                }
            }
        }
    }

    //  방향을 정해놓고 x와 y 값 중 더 큰쪽을 먼저 움직인 후 다음 작은 쪽을 움직인다
    public class Chase : State
    {
        private Animator animator;

        private float detectionRange = 30f;
        private Vector2 targetPos;
        private Vector2 moveDirection;
        private bool isVertical;

        private bool changeDirection;
        private bool isArrive;

        private float chaseTimer = 1f;
        private float chaseCountdown = 0;

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

            targetPos = (Vector2)owner.Target.transform.position + Random.insideUnitCircle * 5f;

            if (Mathf.Abs(owner.transform.position.x - targetPos.x) > Mathf.Abs(owner.transform.position.y - targetPos.y))
            {
                //  왼쪽
                if (targetPos.x < owner.transform.position.x)
                {
                    owner.Dir = Character.Direction.Left;
                }
                else
                {
                    owner.Dir = Character.Direction.Right;
                }
                isVertical = false;
                moveDirection = owner.Move(new Vector3(targetPos.x, owner.transform.position.y, 0f), owner.CurrentMoveSpeed);
            }
            else
            {
                if (targetPos.y < owner.transform.position.y)
                {
                    owner.Dir = Character.Direction.Front;
                }
                else
                {
                    owner.Dir = Character.Direction.Back;
                }
                isVertical = true;
                moveDirection = owner.Move(new Vector3(owner.transform.position.x, targetPos.y, 0f), owner.CurrentMoveSpeed);
            }

            animator.SetInteger("State", 1);
            changeDirection = false;
            chaseCountdown = 0f;
            isArrive = false;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (detectionRange < ((owner.Target.transform.position - owner.transform.position).sqrMagnitude))
            {
                stateMachine.ChangeState(new Idle());
                return;
            }

            if(changeDirection)
            {
                if(isArrive)
                {
                    //  3단계

                    if (((Vector2)owner.transform.position - moveDirection).sqrMagnitude < 0.5f)
                    {
                        stateMachine.ChangeState(new Attack());
                        animator.SetInteger("State", 0);
                    }
                }
                else
                {
                    //  2단계 - 잠깐 대기 후 방향 바꾸기
                    chaseCountdown += deltaTime;

                    if (chaseCountdown > chaseTimer)
                    {
                        isArrive = true;

                        //  세로부터 움직였는가
                        if(isVertical)
                        {
                            //  그 다음은 가로
                            if (targetPos.x < owner.transform.position.x)
                            {
                                owner.Dir = Character.Direction.Left;
                            }
                            else
                            {
                                owner.Dir = Character.Direction.Right;
                            }
                            moveDirection = owner.Move(new Vector3(targetPos.x, owner.transform.position.y, 0f), owner.CurrentMoveSpeed);
                        }
                        else
                        {
                            if (targetPos.y < owner.transform.position.y)
                            {
                                owner.Dir = Character.Direction.Front;
                            }
                            else
                            {
                                owner.Dir = Character.Direction.Back;
                            }
                            moveDirection = owner.Move(new Vector3(owner.transform.position.x, targetPos.y, 0f), owner.CurrentMoveSpeed);
                        }

                        animator.SetInteger("State", 1);
                    }
                }
            }
            else
            {
                //  1단계
                if (((Vector2)owner.transform.position - moveDirection).sqrMagnitude < 0.5f)
                {
                    changeDirection = true;
                    animator.SetInteger("State", 0);
                    owner.StopMoving();
                }
            }
        }
    }

    public class Attack : State
    {
        private Animator animator;

        private float attackTimer = 3f;
        private float attackCountdown = 0f;

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

            owner.StopMoving();
            animator.SetTrigger(AnimationString.attackTrigger);
            attackCountdown = 0f;
        }

        public override void OnUpdate(float deltaTime)
        {
            attackCountdown += deltaTime;

            if(attackCountdown > attackTimer)
            {
                stateMachine.ChangeState(new Idle());
            }
        }
    }
}

