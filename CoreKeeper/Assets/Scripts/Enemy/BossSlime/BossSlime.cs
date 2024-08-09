using UnityEngine;

public class BossSlime : Enemy
{
    public GameObject explosionPrefab;
    public GameObject particlePrefab;

    protected override void Start()
    {
        base.Start();

        currentMoveSpeed = 80f;
    }

    protected override void StateMachineInitialize()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new BossSlimeState.Idle(this, stateMachine));
        stateMachine.RegisterState(new BossSlimeState.Attack(this, stateMachine));

        stateMachine.ChangeState(new BossSlimeState.Idle());
    }

    public override void Die()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool(AnimationString.isDie, true);
        IsDie = true;

        SoundManager.Instance.PlaySfx(SoundManager.Sfx.BossSlimeDie);

        SoundManager.Instance.PlayBgm(SoundManager.Bgm.Nature);

        GameObject obj = Instantiate(particlePrefab, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        Destroy(obj, 3f);
    }

    public void Explosion()
    {
        GameObject obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (dropItems.Length > 0)
        {
            int index = wrp.GetRandomPick();

            GameObject dropItem = Instantiate(Inventory.Instance.dropItemPrefab, transform.position, Quaternion.identity);
            dropItem.GetComponent<DropItem>().SetItemData(dropItems[index]);
        }

        Destroy(gameObject);
    }

    public override void TakeDamage(float _damage, Vector3 _otherPos)
    {
        base.TakeDamage(_damage, _otherPos);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Hit);
    }
}
