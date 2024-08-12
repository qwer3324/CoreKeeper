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

            switch (itemData.info.name)
            {
                case "Wood Table":
                case "Wood Stool":
                case "Wood Workbench":
                case "Wood":
                    SoundManager.Instance.PlaySfx(SoundManager.Sfx.WoodDestroy);
                    break;
                case "Magic Mirror":
                    SoundManager.Instance.PlaySfx(SoundManager.Sfx.MirrorDestroy);
                    break;
                case "Iron Bar":
                    SoundManager.Instance.PlaySfx(SoundManager.Sfx.GlassDestroy);
                    break;
                default:
                    SoundManager.Instance.PlaySfx(SoundManager.Sfx.CropsDestroy);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
