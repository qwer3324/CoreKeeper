using MyFps.Utility;
using UnityEngine;

public class MeinMenuUI : MonoBehaviour
{
    public void GameStart()
    {
        SceneFader.Instance.FadeTo(1);
    }
    
    public void GameExit()
    {
        Application.Quit();
    }
}
