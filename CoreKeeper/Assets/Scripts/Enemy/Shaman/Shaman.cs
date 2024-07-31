using System.Collections;
using UnityEngine;

public class Shaman : Enemy
{
    public GameObject burnPrefab;
    public GameObject missilePrefab;
    public GameObject shootPivot;
    private bool IsRange = true;
    public float attackDamage = 30f;

    protected override void StateMachineInitialize()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new ShamanState.Idle(this, stateMachine));
        stateMachine.RegisterState(new ShamanState.Burn(this, stateMachine));
        stateMachine.RegisterState(new ShamanState.Attack(this, stateMachine));
        stateMachine.RegisterState(new ShamanState.Warp(this, stateMachine));

        stateMachine.ChangeState(new ShamanState.Idle());
    }

    public void ChangeMeleeState()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new ShamanMeleeState.Transform(this, stateMachine));
        stateMachine.RegisterState(new ShamanMeleeState.Idle(this, stateMachine));
        stateMachine.RegisterState(new ShamanMeleeState.Chase(this, stateMachine));
        stateMachine.RegisterState(new ShamanMeleeState.Attack(this, stateMachine));

        stateMachine.ChangeState(new ShamanMeleeState.Transform());
    }

    public void CreateProjectile()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Fireball);

        Vector2 shootDir = (Target.transform.position - shootPivot.transform.position);
        GameObject projectile = Instantiate(missilePrefab, shootPivot.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetProjectile(shootDir, attackDamage);
    }

    public void CreateFireTrap(Vector2 _targetPos)
    {
        Instantiate(burnPrefab, _targetPos, Quaternion.identity);
    }

    public override void Die()
    {
        if(IsRange)
        {
            if (rb != null)
                rb.velocity = Vector2.zero;
            animator.SetInteger("State", 4);
            animator.SetBool("IsRange", false);
            IsRange = false;

            currentHealth = 100f;
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.BossSlimeBerserk);
            ChangeMeleeState();
        }
        else
        {
            if (rb != null)
                rb.velocity = Vector2.zero;
            animator.SetBool("IsDie", true);
            isDie = true;

            StartCoroutine(ItemDrop(2.8f));
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.ShamanMeleeDeath);
            Destroy(gameObject, 3f);
        }
    }

    IEnumerator ItemDrop(float time)
    {
        yield return new WaitForSeconds(time);

        //  아이템 드랍
        if (dropItems.Length > 0)
        {
            int index = wrp.GetRandomPick();

            GameObject obj = Instantiate(Inventory.Instance.dropItemPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<DropItem>().SetItemData(dropItems[index]);
        }
    }

    public override void TakeDamage(float _damage, Vector3 _otherPos)
    {
        base.TakeDamage(_damage, _otherPos);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Hit);
    }
}
