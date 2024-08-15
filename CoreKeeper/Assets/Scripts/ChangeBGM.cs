using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    public SoundManager.Bgm bgm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.PlayBgm(bgm);
    }
}
