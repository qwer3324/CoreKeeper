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
            //���۰� ���ÿ� ���̵� �̹��� ���İ� �ʱ�ȭ
            img.color = new Color(0f, 0f, 0f, 1f);

            //���۰� ���ÿ� ���̵��� ȿ��
            FadeIn(0.2f);
        }

        public void FadeIn(float delayTime)
        {
            StartCoroutine(FadeInTime(delayTime));
        }

        //���̵��� : 1�ʵ���(a:1->a:0)
        IEnumerator FadeInTime(float delayTime)
        {
            //Fade ������ Ÿ��
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

        //���̵�ƿ� : 1�ʵ���(a:0->a:1)
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

            //���̵� �ƿ� �� �� �ε�
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

            //���̵� �ƿ� �� �� �ε�
            SceneManager.LoadScene(sceneNum);
            FadeIn(0f);
        }


        //���̵� �ƿ� �� �� �ε�
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
