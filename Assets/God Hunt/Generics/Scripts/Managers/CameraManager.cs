using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera activeCam;

    [Header("Base Shake Parameters")]
    [SerializeField] public float baseAmplitudeGain = 1;
    [SerializeField] public float baseFrequencyGain = 1;
    [SerializeField] public float baseShakeTime = .2f;

    CinemachineBasicMultiChannelPerlin noise;

    public static CameraManager Instance;
    private void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            DestroyImmediate(this);
    }

    #region API

    public void Setup()
    {
        Singleton();
        CinemachineVirtualCamera[] vCams = FindObjectsOfType<CinemachineVirtualCamera>();
        for (int i = 0; i < vCams.Length; i++)
        {
            vCams[i].AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void SetActiveCamera(CinemachineVirtualCamera _vCam)
    {
        if(activeCam)
            activeCam.enabled = false;
        activeCam = _vCam;
        activeCam.enabled = true;
        noise = activeCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void CamShake(float _amplitude_gain, float _frequency_gain, float _duration_in_seconds)
    {
        StopAllCoroutines();
        noise.m_AmplitudeGain = _amplitude_gain;
        noise.m_FrequencyGain = _frequency_gain;
        StartCoroutine(WaitSecs(_duration_in_seconds));
    }

    public void CamShake()
    {
        StopAllCoroutines();
        noise.m_AmplitudeGain = baseAmplitudeGain;
        noise.m_FrequencyGain = baseFrequencyGain;
        StartCoroutine(WaitSecs(baseShakeTime));
    }


    public void ResetShake()
    {
        StopAllCoroutines();
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    #endregion

    #region Internals

    IEnumerator WaitSecs(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        ResetShake();
    }

    #endregion
}