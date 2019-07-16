using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;

    Selectable[] selectables;
    Slider[] sliders;

    const float DEFAULT_VOLUME = -5;

    private void Awake()
    {
        selectables = GetComponentsInChildren<Selectable>();
        sliders = GetComponentsInChildren<Slider>();
    }

    public void Open()
    {
        selectables[0].Select();
    }

    public void ResetToDefaults()
    {
        mainMixer.SetFloat("MasterVolume", DEFAULT_VOLUME);
        mainMixer.SetFloat("EffectsVolume", DEFAULT_VOLUME);
        mainMixer.SetFloat("MusicVolume", DEFAULT_VOLUME);

        foreach (Slider slider in sliders)
        {
            slider.value = DEFAULT_VOLUME;
        }
    }

    public void SetMasterVolume(float _value)
    {
        mainMixer.SetFloat("MasterVolume", _value);
    }

    public void SetEffectsVolume(float _value)
    {
        mainMixer.SetFloat("EffectsVolume", _value);
    }

    public void SetMusicVolume(float _value)
    {
        mainMixer.SetFloat("MusicVolume", _value);
    }
}