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

        void Start ()
        {
            //시작과 동시에 페이드 이미지 알파값 초기화
            img.color = new Color(0f, 0f, 0f, 1f);

            //시작과 동시에 페이드인 효과
            FadeIn(0.2f);
        }

        public void FadeIn(float delayTime)
        {
            StartCoroutine(FadeInTime(delayTime));
        }

        //페이드인 : 1초동안(a:1->a:0)
        IEnumerator FadeInTime(float delayTime)
        {
            //Fade 딜레이 타임
            if (delayTime > 0)
            {
                yield return new WaitForSeconds(delayTime);
            }

            float countdown = 2f;

            while(countdown > 0f)
            {
                countdown -= Time.deltaTime;
                float a = curve.Evaluate(countdown / 2f);
                img.color = new Color(0f, 0f, 0f, a);
                
                yield return null;
            }
        }

        //페이드아웃 : 1초동안(a:0->a:1)
        IEnumerator FadeOut(string sceneName)
        {
            float countdown = 0f;

            while(countdown < 2f)
            {
                countdown += Time.deltaTime;
                float a = curve.Evaluate(countdown);
                img.color = new Color(0f, 0f, 0f, a);
                
                yield return null;
            }

            //페이드 아웃 후 씬 로드
            SceneManager.LoadScene(sceneName);
        }

        IEnumerator FadeOut(int sceneNum)
        {
            float countdown = 0f;

            while (countdown < 2f)
            {
                countdown += Time.deltaTime;
                float a = curve.Evaluate(countdown);
                img.color = new Color(0f, 0f, 0f, a);

                yield return null;
            }

            //페이드 아웃 후 씬 로드
            SceneManager.LoadScene(sceneNum);
            FadeIn(0f);
        }


        //페이드 아웃 후 씬 로드
        public void FadeTo(string sceneName)
        {
            StartCoroutine(FadeOut(sceneName));
        }

        public void FadeTo(int sceneNum)
        {
            StartCoroutine(FadeOut(sceneNum));
        }
    }
}
