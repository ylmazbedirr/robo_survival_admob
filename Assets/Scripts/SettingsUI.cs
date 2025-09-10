/*
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        sfxSlider.value = AudioManager.Instance.SFXVolume;
    }

    public void OnSFXSliderChanged(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }
}
*/

using UnityEngine;
using UnityEngine.UI;


public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;


    void Start()
    {
        if (AudioManager.Instance != null && sfxSlider != null)
            sfxSlider.value = AudioManager.Instance.SFXVolume;
    }


    public void OnSFXSliderChanged(float volume)
    {
        AudioManager.Instance?.SetSFXVolume(volume);
    }
}
