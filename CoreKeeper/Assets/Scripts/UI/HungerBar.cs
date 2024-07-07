using System.Collections;
using UnityEngine;

public class HungerBar : MonoBehaviour
{
    private Player player;

    private float prevHungerRatio = 1f;
    private float currentHungerRatio = 1f;

    [SerializeField] private SlicedFilledImage hungerImage;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        CharacterEvents.characterFood += UpdateHungerBar;
    }

    public void UpdateHungerBar()
    {
        prevHungerRatio = currentHungerRatio;
        currentHungerRatio = player.CurrentHunger / player.MaxHunger;

        //  허기 게이지가 올라감
        if(prevHungerRatio < currentHungerRatio) 
        {
            StartCoroutine(IncreasedAnimation());
        }
        else
        {
            hungerImage.fillAmount = currentHungerRatio;
        }
    }

    IEnumerator IncreasedAnimation()
    {
        while (prevHungerRatio < currentHungerRatio)
        {
            prevHungerRatio += Time.deltaTime / 2f;

            hungerImage.fillAmount = prevHungerRatio;

            yield return new WaitForEndOfFrame();
        }

        currentHungerRatio = prevHungerRatio;
        hungerImage.fillAmount = currentHungerRatio;
    }
}
