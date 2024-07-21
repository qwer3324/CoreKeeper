using UnityEngine;

public class Projectile : MonoBehaviour
{
    private SpriteRenderer sr;
    private float rangeDamage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rangeDist = 3f;

    private Vector2 dir;

    public string rangeWeaponName;


    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        Destroy(gameObject, rangeDist);
    }

    private void FixedUpdate()
    {
        transform.Translate(dir * moveSpeed  * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character= collision.GetComponent<Character>();

        if (character != null)
        {
            character.TakeDamage(rangeDamage, transform.position);
        }

        switch(gameObject.name)
        {
            case "GalaxiteChakramProjectile":
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.GalaxiteHit);
                break;
            case "CupidArrow":
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.CupidBowHit);
                break;
            case "MusketProjectile":
                //SoundManager.Instance.PlaySfx(SoundManager.Sfx.);
                break;
            case "SlimeProjectile":
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.SlimeProjectile);
                break;
            case "SlingshotProjectile":
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.SlingshotHit);
                break;
            case "WoodArrow":
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.BowHit);
                break;
        }
        Destroy(gameObject);
    }

    public void SetProjectile(Vector2 _shootDir, float _rangeDamage)
    {
        dir = _shootDir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        sr.gameObject.transform.rotation = rotation;

        rangeDamage = _rangeDamage;
    }
}
