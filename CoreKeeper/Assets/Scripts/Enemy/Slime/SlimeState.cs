using System;
using System.Collections;
using UnityEngine;

namespace SlimeState
{
    //  Slime�� ��� ���� : idleRange��ŭ�� �� �ȿ��� �����ϰ� ���� ��� �� �ڸ��� �̵�
    public class Idle : State
    {
        #region Variables
        private float idleTimer = 5f;       //  ��� �ð�
        private float idleMaxTime = 10f;
        private float idleMinTime = 3f;
        private float idleCountdown = 0f;
        private float idleRange = 3f;       //  �̵� �ݰ�

        private bool isArrive = false;      //  ��ǥ ������ �����ߴ���
        private Vector2 targetPos;          //  ��ǥ ����
        #endregion

        public Idle() { }
        public Idle(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnEnter()
        {
            GetRandomPointInCircle();
            targetPos = owner.Move(targetPos, owner.CurrentMoveSpeed);

            idleTimer = UnityEngine.Random.Range(idleMinTime, idleMaxTime);
            idleCountdown = idleTimer;
            isArrive = false;
        }

        public override void OnUpdate(float deltaTime)
        {
            //  �����ߴ�
            if (isArrive)
            {
                idleCountdown -= Time.deltaTime;

                owner.StopMoving();

                if (idleCountdown <= 0)
                {
                    idleTimer = UnityEngine.Random.Range(idleMinTime, idleMaxTime);
                    idleCountdown = idleTimer;
                    isArrive = false;

                    GetRandomPointInCircle();
                    targetPos = owner.Move(targetPos, owner.CurrentMoveSpeed);
                }
            }
            //  ��ǥ���� �����̴� ��
            else
            {
                if (((Vector2)owner.transform.position - targetPos).sqrMagnitude < 0.5f)
                {
                    isArrive = true;
                }
            }
        }

        private void GetRandomPointInCircle()
        {
            targetPos = (Vector2)owner.transform.position + UnityEngine.Random.insideUnitCircle * idleRange;
        }
    }

    public class Chase : State
    {
        #region Variables
        private float attackTimer = 0.5f;
        private float attackCountdown = 0f;
        private float chaseDist = 80f;      //  ���� ����
        private float attackDist = 10f;     //  ���� ����
        private float chaseSpeed = 80f;
        #endregion

        public Chase() { }
        public Chase(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnEnter()
        {
            if ((owner.Target.transform.position - owner.transform.position).sqrMagnitude < attackDist)
            {
                //���� ���� ���̸�
                stateMachine.ChangeState(new Attack());
                return;
            }

            attackCountdown = attackTimer;
        }

        public override void OnUpdate(float deltaTime)
        {
            owner.Move(owner.Target.transform.position, chaseSpeed);

            if ((owner.Target.transform.position - owner.transform.position).sqrMagnitude > chaseDist)
            {
                stateMachine.ChangeState(new Idle());
                return;
            }

            attackCountdown -= Time.deltaTime;

            if (attackCountdown <= 0)
            {
                if ((owner.Target.transform.position - owner.transform.position).sqrMagnitude < attackDist)
                {
                    //���� ���� ���̸�
                    stateMachine.ChangeState(new Attack());
                    return;
                }
            }
        }
    }

    public class Attack : State
    {
        private Animator animator;
        private GameObject renderer;

        private Vector2 targetPos;
        private Vector2 startPos;

        private float attackTimer;
        private float attackDelay = 1f; //  ���� ��������
        private float delayTimer;
        private float afterDelay = 1f;  //  ���� �ĵ�����

        private bool isAttack;
        private bool isDone;

        public Attack() { }

        public Attack(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnInitialize()
        {
            animator = owner.GetComponent<Animator>();
            renderer = owner.transform.GetChild(0).gameObject;
        }

        public override void OnEnter()
        {
            SetTargetPos();
            startPos = owner.transform.position;
            attackTimer = 0f;
            delayTimer = attackDelay;
            isAttack = false;
            isDone = false;
            animator.SetTrigger(AnimationString.attackTrigger);
            owner.StopMoving();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (isAttack)
            {
                if (isDone)
                {
                    delayTimer -= deltaTime;

                    if (delayTimer <= 0f)
                    {
                        stateMachine.ChangeState(new Chase());
                        return;
                    }
                }
            }
            else
            {
                delayTimer -= deltaTime;

                if (delayTimer <= 0f)
                {
                    isAttack = true;
                    delayTimer = afterDelay;
                    CoroutineHelper.StartCoroutine(JumpAttack());
                }
            }
        }

        IEnumerator JumpAttack()
        {
            attackTimer = 0;

            //�߰� ���� = A + (B - A) * ����
            while (attackTimer < 1f)
            {
                attackTimer += Time.deltaTime;
                renderer.transform.position = Parabola(startPos, targetPos, 1f, attackTimer);
                float Y = startPos.y + (targetPos.y - startPos.y) * attackTimer;
                owner.transform.position = new Vector2(renderer.transform.position.x, Y);
                yield return new WaitForEndOfFrame();
            }

            renderer.transform.position = owner.transform.position;
            isDone = true;
        }

        private void SetTargetPos()
        {
            targetPos = owner.Target.transform.position;

            //  Ÿ���� ������ �����ʿ� �ִ�
            if(startPos.x < targetPos.x)
            {
                //��¦ ������ ��ǥ��
                targetPos.x = targetPos.x - 0.01f;
            }
            else
            {
                targetPos.x = targetPos.x + 0.01f;
            }

            //  Ÿ���� ������ ���� �ִ�
            if(startPos.y < targetPos.y)
            {
                targetPos.y = targetPos.y - 0.01f;
            }
            else
            {
                targetPos.y = targetPos.y + 0.05f;
            }
        }

        #region Parabola
        /// <summary>
        /// ������ �׸��� �Լ�
        /// </summary>
        /// <param name="start">���� ����</param>
        /// <param name="end">���� ����</param>
        /// <param name="height">�������� ����</param>
        /// <param name="t">�ð�</param>
        /// <returns></returns>
        private Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
            var mid = Vector3.Lerp(start, end, t);
            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }
        #endregion
    }
}
