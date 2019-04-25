using Cinemachine;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] NoiseSettings noiseType;

    List<CinemachineVirtualCamera> cams;
    CinemachineVirtualCamera currentCam;
    CinemachineBasicMultiChannelPerlin currentNoise;

    public void Init()
    {
        if (Instance == null)
            Instance = this;

        cams = new List<CinemachineVirtualCamera>();
        cams = FindObjectsOfType<CinemachineVirtualCamera>().ToList();

        DisableAllCams();
    }

    public void ChangeActiveCam(CinemachineVirtualCamera _newCam)
    {
        DisableAllCams();

        _newCam.enabled = true;
        currentCam = _newCam;
    }

    public void CameraShake(float _frequencyGain, float _amplitudeGain,  float _duration)
    {
        currentNoise = currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (currentNoise == null)
        {
            currentNoise = currentCam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        currentNoise.m_NoiseProfile = noiseType;
        currentNoise.m_AmplitudeGain = _amplitudeGain;
        currentNoise.m_FrequencyGain = _frequencyGain;
        TimerGod.Timer(_duration, StopShake);
        print("madonna che cam shake impressionante");
    }

    public void StopShake()
    {
        currentNoise.m_AmplitudeGain = 0;
        currentNoise.m_FrequencyGain = 0;
    }

    void DisableAllCams()
    {
        for (int i = 0; i < cams.Count; i++)
        {
            cams[i].enabled = false;
        }
    }

}