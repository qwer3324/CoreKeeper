using UnityEngine;

public class MouseSelect : MonoBehaviour
{
    private SpriteRenderer sr;

    private Vector2 mousePosition;

    [SerializeField] private Sprite redBox;
    [SerializeField] private Sprite blueBox;

    public Item placedItem;
    public int placedItemIndex;

    public GameObject woodWorkbenchPrefab;
    public GameObject woodTablePrefab;
    public GameObject woodStoolPrefab;
    public GameObject cookingPotPrefab;
    public GameObject magicMirrorPrefab;
    public GameObject torchPrefab;
    public GameObject anvilPrefab;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        mousePosition = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        transform.position = mousePosition;

        if (Mathf.Abs(transform.localPosition.x) > 1.5f || Mathf.Abs(transform.localPosition.y) > 1.5f)
        {
            sr.sprite = redBox;
        }
        else
        {
            //int layerMask = ~0 & ~((1 << 6) | (1 << 14));
            int layerMask = (1 << 4) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 13) | (1 << 14);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 5f, layerMask);

            if (hit.collider == null)
            {
                sr.sprite = blueBox;
            }
            else
            {
                sr.sprite = redBox;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Installation();
            }
        }

        if(Input.GetMouseButtonDown(1)) 
        { 
            UIManager.Instance.ChangeUiMode(UIManager.UI_Mode.Normal);
        }
    }

    public void Installation()
    {
        switch (placedItem.name)
        {
            case "Wood Workbench":
                Instantiate(woodWorkbenchPrefab, mousePosition, Quaternion.identity);
                break;
            case "Wood Table":
                Instantiate(woodTablePrefab, mousePosition, Quaternion.identity);
                break;
            case "Wood Stool":
                Instantiate(woodStoolPrefab, mousePosition, Quaternion.identity);
                break;
            case "Cooking Pot":
                Instantiate(cookingPotPrefab, mousePosition, Quaternion.identity);
                break;
            case "Magic Mirror":
                Instantiate(magicMirrorPrefab, mousePosition, Quaternion.identity);
                break;
            case "Torch":
                Instantiate(torchPrefab, mousePosition, Quaternion.identity);
                break;
            case "Anvil":
                Instantiate(anvilPrefab, mousePosition, Quaternion.identity);
                break;
        }
        UIManager.Instance.ChangeUiMode(UIManager.UI_Mode.Normal);
        Inventory.Instance.RemoveItem(placedItem, placedItemIndex);
        gameObject.SetActive(false);
    }
}
