using UnityEngine;

public class Placement : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
        {
            GameObject obj = Instantiate(Inventory.Instance.dropItemPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<DropItem>().SetItemData(itemData);
            Destroy(gameObject);
        }
    }
}
