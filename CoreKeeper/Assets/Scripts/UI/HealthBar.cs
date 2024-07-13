using System.Collections;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Player player;

    private float prevHealthRatio = 1f;
    private float currentHealthRatio = 1f;

    [SerializeField] private SlicedFilledImage topImage;
    [SerializeField] private SlicedFilledImage bottomImage;

    public Material whiteFlashMaterial;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        Equipment.Instance.OnEquipmentChanged += UpdateHealthBar;
        CharacterEvents.playerDamaged += Damaged;
        CharacterEvents.characterHeal += Heal;
    }

    public void UpdateHealthBar()
    {
        currentHealthRatio = player.CurrentHealth / player.MaxHealth;

        topImage.fillAmount = currentHealthRatio;
        bottomImage.fillAmount = currentHealthRatio;
    }

    public void Damaged(GameObject _obj, float _amount)
    {
        currentHealthRatio = player.CurrentHealth / player.MaxHealth;

        if(_amount > 0)
            StartCoroutine(WhiteFlashAnimation());
    }

    public void Heal(GameObject _obj, float _amount)
    {
        prevHealthRatio = currentHealthRatio;
        currentHealthRatio = player.CurrentHealth / player.MaxHealth;

        StartCoroutine(IncreasedAnimation());
    }

    //  하얗게 번쩍이는 애니메이션
    IEnumerator WhiteFlashAnimation()
    {
        topImage.material = whiteFlashMaterial;
        bottomImage.material = whiteFlashMaterial;

        yield return new WaitForSeconds(0.1f);

        topImage.material = null;
        bottomImage.material = null;

        topImage.fillAmount = currentHealthRatio;
        bottomImage.fillAmount = currentHealthRatio;
    }

    //  서서히 올라가는 애니메이션
    IEnumerator IncreasedAnimation()
    {
        while(prevHealthRatio < currentHealthRatio)
        {
            prevHealthRatio += Time.deltaTime;

            topImage.fillAmount = prevHealthRatio;
            bottomImage.fillAmount = prevHealthRatio;

            yield return new WaitForEndOfFrame();
        }

        currentHealthRatio = prevHealthRatio;
        topImage.fillAmount = currentHealthRatio;
        bottomImage.fillAmount = currentHealthRatio;
    }
}
