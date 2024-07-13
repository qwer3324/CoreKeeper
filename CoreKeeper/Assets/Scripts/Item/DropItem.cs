using UnityEngine;

public class DropItem : MonoBehaviour
{
    private SpriteRenderer sr;

    public ItemData data;
    [SerializeField] private Item item;
    private Transform target;
    private float timer = 0f;
    private bool isTrace = true;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetItemData(ItemData _data)
    {
        Item newItem = _data.CreateItem();
        item = newItem;
        sr.sprite = Inventory.Instance.itemDB.Datas[item.id].dropIcon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject.transform;
        timer = 0f;
        isTrace = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isTrace)
        {
            if ((target.position - transform.position).sqrMagnitude <= 0.05f)
            {
                if (Inventory.Instance.AddItem(item))
                {
                    Destroy(gameObject);
                }
                else
                    isTrace = false;
            }
            else
            {
                timer += Time.deltaTime;

                if(timer > 0.3f)
                    transform.position = Vector3.Lerp(transform.position, target.position, timer / 8f);
            }
        }
    }
}
