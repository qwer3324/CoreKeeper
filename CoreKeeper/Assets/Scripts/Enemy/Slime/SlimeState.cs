using System;
using System.Collections;
using UnityEngine;

namespace SlimeState
{
    //  Slime의 대기 상태 : idleRange만큼의 원 안에서 랜덤하게 점을 찍어 그 자리로 이동
    public class Idle : State
    {
        #region Variables
        private float idleTimer = 5f;       //  대기 시간
        private float idleMaxTime = 10f;
        private float idleMinTime = 3f;
        private float idleCountdown = 0f;
        private float idleRange = 3f;       //  이동 반경

        private bool isArrive = false;      //  목표 지점에 도착했는지
        private Vector2 targetPos;          //  목표 지점
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
            //  도착했다
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
            //  목표까지 움직이는 중
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
        private float chaseDist = 80f;      //  추적 범위
        private float attackDist = 10f;     //  공격 범위
        private float chaseSpeed = 80f;
        #endregion

        public Chase() { }
        public Chase(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnEnter()
        {
            if ((owner.Target.transform.position - owner.transform.position).sqrMagnitude < attackDist)
            {
                //공격 범위 안이면
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
                    //공격 범위 안이면
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
        private float attackDelay = 1f; //  공격 선딜레이
        private float delayTimer;
        private float afterDelay = 1f;  //  공격 후딜레이

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

            //중간 지점 = A + (B - A) * 비율
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

            //  타겟이 나보다 오른쪽에 있다
            if(startPos.x < targetPos.x)
            {
                //살짝 왼쪽을 목표로
                targetPos.x = targetPos.x - 0.01f;
            }
            else
            {
                targetPos.x = targetPos.x + 0.01f;
            }

            //  타겟이 나보다 위에 있다
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
        /// 포물선 그리는 함수
        /// </summary>
        /// <param name="start">시작 지점</param>
        /// <param name="end">도착 지점</param>
        /// <param name="height">포물선의 고점</param>
        /// <param name="t">시간</param>
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
