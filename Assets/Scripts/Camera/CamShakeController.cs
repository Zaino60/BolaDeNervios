using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamShakeController : MonoBehaviour
{
    private CinemachineFreeLook cam;
    private CinemachineBasicMultiChannelPerlin perlinNoiseTop, perlinNoiseMiddle, perlinNoiseBottom;

    void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();
        perlinNoiseTop = cam.GetRig(0).GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        perlinNoiseMiddle = cam.GetRig(1).GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        perlinNoiseBottom = cam.GetRig(2).GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
    }

    public void CameraShake(float intensity, float time)
    {
        perlinNoiseTop.m_AmplitudeGain = intensity;
        perlinNoiseMiddle.m_AmplitudeGain = intensity;
        perlinNoiseBottom.m_AmplitudeGain = intensity;
        StartCoroutine(ShakeTime(time));
        Debug.Log("Shake START");
    }

    IEnumerator ShakeTime(float _time)
    {
        yield return new WaitForSeconds(_time);
        ResetIntensity();
    }

    void ResetIntensity()
    {
        perlinNoiseTop.m_AmplitudeGain = 0f;
        perlinNoiseMiddle.m_AmplitudeGain = 0f;
        perlinNoiseBottom.m_AmplitudeGain = 0f;
        Debug.Log("Shake END");
    }
}
