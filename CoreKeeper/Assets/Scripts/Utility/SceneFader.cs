using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyFps.Utility
{
    public class SceneFader : SingletonBehaviour<SceneFader>
    {
        public Image img;
        public AnimationCurve curve;
        private float timer = 0f;
        private float delayTime = 1f;
        private float fadeTime = 1f;

        void Start ()
        {
            //시작과 동시에 페이드 이미지 알파값 초기화
            img.color = new Color(0f, 0f, 0f, 1f);

            //시작과 동시에 페이드인 효과
            FadeIn();
        }

        public void FadeOut(int sceneNumber = -1)
        {
            StartCoroutine(FadeOutTime(sceneNumber));
        }

        IEnumerator FadeOutTime(int sceneNumber)
        {
            //Fade 딜레이 타임
            if (delayTime > 0)
            {
                yield return new WaitForSeconds(delayTime);
            }

            Color alpha = img.color;

            while(alpha.a < 1f)
            {
                timer += Time.deltaTime / fadeTime;
                alpha.a = curve.Evaluate(timer);
                img.color = alpha;
                yield return null;
            }

            timer = 0f;

            if (sceneNumber > -1)
            {
                SceneManager.LoadScene(sceneNumber);
                FadeIn();
            }
            else
            {
                GameObject player = GameObject.FindWithTag("Player");

                if (player != null) 
                {
                    player.GetComponent<Player>().Init();
                    FadeIn();
                }
            }
        }

        public void FadeIn()
        {
            StartCoroutine(FadeInTime());
        }

        IEnumerator FadeInTime()
        {
            yield return new WaitForSeconds(delayTime);

            Color alpha = img.color;

            while (alpha.a > 0f)
            {
                timer += Time.deltaTime / fadeTime;
                alpha.a = curve.Evaluate(1 - timer);
                img.color = alpha;
                yield return null;
            }

            timer = 0f;
        }
    }
}
