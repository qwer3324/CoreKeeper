using System;
using System.Collections;
using UnityEngine;

namespace BossSlimeState
{
    public class Idle : State
    {
        private float detectionDist = 50f;

        public Idle() { }
        public Idle(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine) { }

        public override void OnUpdate(float deltaTime)
        {
            if((owner.Target.transform.position - owner.transform.position).sqrMagnitude < detectionDist) 
            {
                stateMachine.ChangeState(new Attack());
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
        private float attackDelay = 0.5f; //  공격 선딜레이
        private float delayTimer;
        private float afterDelay = 1.5f;  //  공격 후딜레이

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
            SoundManager.Instance.PlayBgm(SoundManager.Bgm.BossSlime);
            targetPos = owner.Target.transform.position;
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
                        stateMachine.ChangeState(new Idle());
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

        public override void OnExit()
        {
            base.OnExit();

            SoundManager.Instance.PlayBgm(SoundManager.Bgm.Nature);
        }

        IEnumerator JumpAttack()
        {
            attackTimer = 0;
            targetPos = owner.Target.transform.position;

            //중간 지점 = A + (B - A) * 비율
            while (attackTimer < 1f)
            {
                attackTimer += Time.deltaTime * 2f;
                renderer.transform.position = Parabola(startPos, targetPos, 2f, attackTimer);
                float Y = startPos.y + (targetPos.y - startPos.y) * attackTimer;
                owner.transform.position = new Vector2(renderer.transform.position.x, Y);
                yield return new WaitForEndOfFrame();
            }

            renderer.transform.position = owner.transform.position;
            isDone = true;
            CoroutineHelper.StartCoroutine(CameraController.Instance.Shake(0.3f));  //  카메라 진동
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.BossSlimeAttack);
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