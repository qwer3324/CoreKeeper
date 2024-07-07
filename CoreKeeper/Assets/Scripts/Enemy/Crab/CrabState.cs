using UnityEngine;

namespace CrabState
{
    public class Idle : State
    {
        private Animator animator;

        #region Variables
        private float idleTimer = 5f;       //  ��� �ð�
        private float idleMaxTime = 10f;
        private float idleMinTime = 3f;
        private float idleCountdown = 0f;
        private float minRange = 3f;       //  �̵� �ݰ�
        private float maxRange = 6f;

        private bool isArrive = false;      //  ��ǥ ������ �����ߴ���
        private Vector2 targetPos;          //  ��ǥ ����

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

            //  �����ߴ�
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
            //  ��ǥ���� �����̴� ��
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

    //  ������ ���س��� x�� y �� �� �� ū���� ���� ������ �� ���� ���� ���� �����δ�
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
                //  ����
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
                    //  3�ܰ�

                    if (((Vector2)owner.transform.position - moveDirection).sqrMagnitude < 0.5f)
                    {
                        stateMachine.ChangeState(new Attack());
                        animator.SetInteger("State", 0);
                    }
                }
                else
                {
                    //  2�ܰ� - ��� ��� �� ���� �ٲٱ�
                    chaseCountdown += deltaTime;

                    if (chaseCountdown > chaseTimer)
                    {
                        isArrive = true;

                        //  ���κ��� �������°�
                        if(isVertical)
                        {
                            //  �� ������ ����
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
                //  1�ܰ�
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

