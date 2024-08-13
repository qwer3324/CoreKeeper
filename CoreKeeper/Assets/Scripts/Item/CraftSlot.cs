using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CraftData data;
    public Image icon;
    public DescriptionUI description;

    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.Instance;
        icon.sprite = inventory.itemDB.Datas[data.itemID].dropIcon;
    }

    private void OnDisable()
    {
        description.gameObject.SetActive(false);
    }

    public void CraftItem()
    {
        int[] indexs = new int[data.materialsID.Length];

        for (int i = 0; i < data.materialsID.Length; i++) 
        {
            indexs[i] = inventory.SearchItem(data.materialsID[i], data.materialsAmount[i]);

            if(indexs[i] < 0)
            {
                SoundManager.Instance.PlaySfx(SoundManager.Sfx.MenuDeny);
                return;
            }
        }

        for(int i = 0; i < data.materialsID.Length; i++)
        {
            inventory.RemoveItem(inventory.Items[indexs[i]], indexs[i], data.materialsAmount[i]);
        }
        inventory.AddItem(inventory.itemDB.Datas[data.itemID].CreateItem());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.gameObject.SetActive(true);
        description.transform.position = transform.position + new Vector3(240f, -170f, 0f);
        description.UpdateDescriptionUI(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.gameObject.SetActive(false);
    }
}
