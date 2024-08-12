using MyFps.Utility;
using UnityEngine;

public class MeinMenuUI : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBgm(SoundManager.Bgm.MainMenu);
    }

    public void GameStart()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.MenuSelect);
        SceneFader.Instance.FadeOut(1);
    }
    
    public void GameExit()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.MenuSelect);
        Application.Quit();
    }
}
