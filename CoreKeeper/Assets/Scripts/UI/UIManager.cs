using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : SingletonBehaviour<UIManager>
{
    #region ����
    private Canvas overlayCanvas;
    private Canvas worldCanvas;
    private Player player;
    #endregion

    public enum UI_Type { Equipment, Inventory, Crafting, Information, Status, Description ,Health, Hunger, MouseSelect }
    public enum UI_Mode { Normal, Inventory, Workbench, Cooking, Placed, Customizing }

    private UI_Mode PrevMode = UI_Mode.Normal;
    public UI_Mode CurrentMode = UI_Mode.Normal;

    #region Damage Text
    [Header("Damage Text UI")]
    [Tooltip("������ ���� �� �ؽ�Ʈ ������")]
    public GameObject healthTextPrefab;
    [Tooltip("������ �ؽ�Ʈ ������")]
    [SerializeField] private Vector3 offset;
    [Tooltip("������ �ؽ�Ʈ ���� ������")]
    [SerializeField] private Vector2 randomOffset;
    [Header("Damage Text Materials")]
    public Material enemyDamageTextMaterial;
    public Material playerDamageTextMaterial;
    public Material healTextMaterial;
    public Material dodgeTextMaterial;
    public Material criticalTextMaterial;
    #endregion

    public GameObject hitEffectPrefab;

    [Header("Information UI")]
    [Tooltip("��� UI"), SerializeField] private GameObject equipmentUI;
    [Tooltip("�κ��丮 UI"), SerializeField] private GameObject inventoryUI;
    [Tooltip("������ �� ���� UI"), SerializeField] private GameObject informationUI;
    [Tooltip("ĳ���� ���� UI"), SerializeField] private GameObject statusUI;
    [Tooltip("�⺻ ���� UI"), SerializeField] private GameObject craftingUI;
    [Tooltip("���� ���� UI"), SerializeField] private GameObject descriptionUI;

    [Tooltip("�۾��� UI"), SerializeField, Space(10)] private GameObject workbenchUI;
    [Tooltip("�丮 UI"), SerializeField] private GameObject cookingUI;
    [Tooltip("Ŀ���͸���¡ UI"), SerializeField] private GameObject customizingUI;

    [Header("Bar UI")]
    [Tooltip("ü�� UI"), SerializeField] private GameObject healthBarUI;
    [Tooltip("��� UI"), SerializeField] private GameObject hungerBarUI;

    [Tooltip("���콺 ����"), SerializeField, Space(10)] private GameObject mouseSelectUI;

    private new void Awake()
    {
        base.Awake();

        overlayCanvas = GameObject.Find("OverlayCanvas").GetComponent<Canvas>();
        worldCanvas = GameObject.Find("WorldCanvas").GetComponent<Canvas>();
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        //UnityAction �Լ� ���
        CharacterEvents.playerDamaged += PlayerTakeDamage;
        CharacterEvents.enemyDamaged += EnemyTakeDamage;
        CharacterEvents.characterHeal += CharacterHeal;
        CharacterEvents.enemyDamaged += InstantiateHitEffect;
    }

    private void OnDisable()
    {
        //UnityAction �Լ� ����
        CharacterEvents.playerDamaged -= PlayerTakeDamage;
        CharacterEvents.enemyDamaged -= EnemyTakeDamage;
        CharacterEvents.characterHeal -= CharacterHeal;
        CharacterEvents.enemyDamaged -= InstantiateHitEffect;
    }

    private TextMeshProUGUI CreateHealthText(GameObject character)
    {
        //Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position) + offset;
        Vector3 spawnPosition = character.transform.position + offset;
        spawnPosition.x += Random.Range(-randomOffset.x, randomOffset.x);
        spawnPosition.y += Random.Range(-randomOffset.y, randomOffset.y);

        GameObject textGo = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, worldCanvas.transform);
        TextMeshProUGUI healthText = textGo.GetComponentInChildren<TextMeshProUGUI>();

        return healthText;
    }

    /// <summary>
    /// �÷��̾ ������ �Ծ��� �� �ؽ�Ʈ ����
    /// </summary>
    /// <param name="character">�÷��̾� ������Ʈ</param>
    /// <param name="damageReceived">���� ������</param>
    public void PlayerTakeDamage(GameObject character, float damageReceived)
    {
        TextMeshProUGUI healthText = CreateHealthText(character);

        if (healthText != null)
        {
            //  ȸ������ ��(damageReceived�� -1�� ���� ��)
            if (damageReceived < 0)
            {
                healthText.text = "miss";
                healthText.fontMaterial = dodgeTextMaterial;
            }
            else
            {
                healthText.text = ((int)damageReceived).ToString();
                healthText.fontMaterial = playerDamageTextMaterial;
            }
        }
    }

    public void EnemyTakeDamage(GameObject character, float damageReceived)
    {
        TextMeshProUGUI healthText = CreateHealthText(character);

        if (healthText != null)
        {
            //  ũ��Ƽ�� ���� ��(damageReceived�� ������ ���� ��)
            if(damageReceived < 0)
            {
                healthText.text = ((int)-damageReceived).ToString();
                healthText.fontMaterial = criticalTextMaterial;
            }
            else
            {
                healthText.text = ((int)damageReceived).ToString();
                healthText.fontMaterial = enemyDamageTextMaterial;
            }
        }
    }

    public void CharacterHeal(GameObject character, float healValue)
    {
        TextMeshProUGUI healthText = CreateHealthText(character);

        if (healthText != null)
        {
            healthText.text = ((int)healValue).ToString();
            healthText.fontMaterial = healTextMaterial;
        }
    }

    public void OnWindowActive(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ChangeUiMode(UI_Mode.Inventory);
        }
    }

    public void InstantiateHitEffect(GameObject character, float damageReceived)
    {
        Vector2 spawnPosition = character.transform.position;
        spawnPosition.x += Random.Range(-0.7f, 0.7f);
        spawnPosition.y += Random.Range(-0.4f, 1f);

        Instantiate(hitEffectPrefab, spawnPosition, Quaternion.identity);
    }

    public GameObject GetUI(UI_Type _type)
    {
        switch (_type)
        {
            case UI_Type.Equipment:
                return equipmentUI;
            case UI_Type.Inventory:
                return inventoryUI;
            case UI_Type.Crafting:
                return craftingUI;
            case UI_Type.Information:
                return informationUI;
            case UI_Type.Status:
                return statusUI;
            case UI_Type.Description:
                return descriptionUI;
            case UI_Type.Health:
                return healthBarUI;
            case UI_Type.Hunger:
                return hungerBarUI;
            default:
                Debug.Log("�������� �ʴ� UI ���� �õ�");
                return null;
        }
    }

    public void SetPlacedItem(Item _item, int _index)
    {
        mouseSelectUI.GetComponent<MouseSelect>().placedItem = _item;
        mouseSelectUI.GetComponent<MouseSelect>().placedItemIndex = _index;
        ChangeUiMode(UI_Mode.Placed);
    }

    public void ChangeUiMode(UI_Mode _mode)
    {
        PrevMode = CurrentMode;

        switch (PrevMode)
        {
            case UI_Mode.Normal:
                break;
            case UI_Mode.Inventory:
                equipmentUI.SetActive(false);
                inventoryUI.SetActive(false);
                craftingUI.SetActive(false);
                statusUI.SetActive(false);
                break;
            case UI_Mode.Workbench:
                workbenchUI.SetActive(false);
                inventoryUI.SetActive(false);
                break;
            case UI_Mode.Cooking:
                cookingUI.SetActive(false);
                inventoryUI.SetActive(false);
                break;
            case UI_Mode.Placed:
                mouseSelectUI.SetActive(false);
                break;
            case UI_Mode.Customizing:
                customizingUI.SetActive(false);
                break;

        }
        if (PrevMode == _mode)
        {
            CurrentMode = UI_Mode.Normal;
            player.CantMove = false;
            return;
        }

        CurrentMode = _mode;
        player.CantMove = true;

        switch (CurrentMode)
        {
            case UI_Mode.Normal:
                player.CantMove = false;
                break;
            case UI_Mode.Inventory:
                equipmentUI.SetActive(true);
                inventoryUI.SetActive(true);
                craftingUI.SetActive(true);
                statusUI.SetActive(true);
                break;
            case UI_Mode.Workbench:
                workbenchUI.SetActive(true);
                inventoryUI.SetActive(true);
                break;
            case UI_Mode.Cooking:
                cookingUI.SetActive(true);
                inventoryUI.SetActive(true);
                break;
            case UI_Mode.Placed:
                mouseSelectUI.SetActive(true);
                break;
            case UI_Mode.Customizing:
                customizingUI.SetActive(true);
                break;
        }
    }
}
