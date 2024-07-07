using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Vector2 dir;
    private float lifeTimer = 5f;
    private float lifeCountdown = 0;
    private float slowTimer = 3f;
    public float moveSpeed = 30f;
    public float attackDamage = 10f;
    public GameObject effectPrefab;
    private bool isSlow = false;


    private void Start()
    {
        float randomAngle = Random.Range(-45f, 45f);

        // ·£´ýÇÑ ¹æÇâ º¤ÅÍ »ý¼º
        dir = Quaternion.Euler(0, 0, randomAngle) * dir;

        transform.GetComponent<Rigidbody2D>().velocity = dir * moveSpeed * Time.deltaTime;

        lifeTimer = Random.Range(4f, 7f);
        slowTimer = lifeTimer / 2 + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeCountdown += Time.deltaTime;

        if(!isSlow)
        {
            if (lifeCountdown > slowTimer)
            {
                transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                isSlow= true;
            }
        }
        else
        {
            if (lifeCountdown > lifeTimer)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();

        if(character != null) 
        {
            character.TakeDamage(attackDamage, transform.position);
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
