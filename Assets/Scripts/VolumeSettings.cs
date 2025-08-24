/*
// File: VolumeSettings.cs
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    private AudioSource musicSource;

    private const string VolumeKey = "MusicVolume";

    private float baseMusicVolume = 0.25f; // Müzik dosyasının kendine ait default sesi

    void Start()
    {
        GameObject musicObj = GameObject.FindGameObjectWithTag("Music");

        if (musicObj != null)
        {
            musicSource = musicObj.GetComponent<AudioSource>();

            float savedVolume = PlayerPrefs.HasKey(VolumeKey)
                ? PlayerPrefs.GetFloat(VolumeKey)
                : 1f;

            musicSource.volume = baseMusicVolume * savedVolume;

            if (musicSlider != null)
            {
                musicSlider.value = savedVolume;
                musicSlider.onValueChanged.AddListener(OnVolumeChanged);
            }
        }
        else
        {
            Debug.LogWarning("Music object with 'Music' tag not found.");
        }
    }

    private void OnVolumeChanged(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = baseMusicVolume * value;
            PlayerPrefs.SetFloat(VolumeKey, value);
            PlayerPrefs.Save();
        }
    }
}

*/


using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    private AudioSource musicSource;
    private const string VolumeKey = "MusicVolume";
    private float baseMusicVolume = 0.25f;


    void Start()
    {
        GameObject musicObj = GameObject.FindGameObjectWithTag("Music");
        if (musicObj != null)
        {
            musicSource = musicObj.GetComponent<AudioSource>();
            float savedVolume = PlayerPrefs.HasKey(VolumeKey) ? PlayerPrefs.GetFloat(VolumeKey) : 1f;
            if (musicSource != null) musicSource.volume = baseMusicVolume * savedVolume;
            if (musicSlider != null)
            {
                musicSlider.value = savedVolume;
                musicSlider.onValueChanged.AddListener(OnVolumeChanged);
            }
        }
        else
        {
            Debug.LogWarning("Music object with 'Music' tag not found.");
        }
    }

    private void OnVolumeChanged(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = baseMusicVolume * value;
            PlayerPrefs.SetFloat(VolumeKey, value); PlayerPrefs.Save();
        }
    }
}