using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    private Player player;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI criticalText;
    public TextMeshProUGUI criticalDamageText;
    public TextMeshProUGUI moveSpeedText;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        UpdateStatusUI();

        Equipment.Instance.OnEquipmentChanged += UpdateStatusUI;
    }

    private void UpdateStatusUI()
    {
        healthText.text = $"{player.MaxHealth}";
        defenceText.text = $"{player.DefencePower}";
        dodgeText.text = $"{player.DodgeChance} %";
        attackText.text = $"{player.CurrentAttackDamage}";
        criticalText.text = $"{player.CriticalHitChance} %";
        criticalDamageText.text = $"{player.CriticalHitDamage} น่";
        moveSpeedText.text = $"{player.CurrentMoveSpeed}";
    }
}
