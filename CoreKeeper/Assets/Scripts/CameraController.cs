using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : SingletonBehaviour<CameraController>
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private new void Awake()
    {
        base.Awake();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator Shake(float _time)
    {
        noise.m_AmplitudeGain = 1f;
        noise.m_FrequencyGain = 1f;

        yield return new WaitForSeconds(_time);

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
