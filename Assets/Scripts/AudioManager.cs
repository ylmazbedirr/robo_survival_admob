/*

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Range(0f, 1f)]
    public float SFXVolume = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        PlayerPrefs.Save();
    }
}
*/


using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    //
    [SerializeField] private AudioSource[] audioPool;
    private int audioIndex = 0;


    [Range(0f, 1f)] public float SFXVolume = 1f;


    [Header("One-shot Pool")]
    [SerializeField, Min(1)] private int poolSize = 12;
    [SerializeField] private float base3DMaxDistance = 70f;
    [SerializeField] private float base3DMinDistance = 2f;


    private AudioSource[] _oneShots;
    private int _cursor;


    void Awake()
    {

        //
        audioPool = new AudioSource[10];
        for (int i = 0; i < audioPool.Length; i++)
        {
            GameObject go = new GameObject("SFX_" + i);
            go.transform.parent = transform;
            audioPool[i] = go.AddComponent<AudioSource>();
        }


        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);


        // Pre-warm a few 3D AudioSources to avoid tiny GC spikes on deaths.
        _oneShots = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            var node = new GameObject($"SFX_{i}");
            node.transform.SetParent(transform, false);
            var src = node.AddComponent<AudioSource>();
            src.playOnAwake = false; src.spatialBlend = 1f; src.rolloffMode = AudioRolloffMode.Linear;
            src.minDistance = base3DMinDistance; src.maxDistance = base3DMaxDistance;
            _oneShots[i] = src;
        }
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        PlayerPrefs.Save();
    }


    /*
        /// <summary>Cheap 3D one-shot (no transient GameObject allocs).</summary>
        public void PlayAt(AudioClip clip, Vector3 pos, float volumeScale = 1f, float pitch = 1f)
        {
            if (clip == null || _oneShots == null || _oneShots.Length == 0) return;
            var src = _oneShots[_cursor++ % _oneShots.Length];
            src.transform.position = pos;
            src.pitch = Mathf.Clamp(pitch, 0.5f, 2f);
            src.volume = Mathf.Clamp01(SFXVolume * volumeScale);
            src.PlayOneShot(clip);

        }
    }

    */
    public void PlayAt(AudioClip clip, Vector3 pos, float volume = 1f, float pitch = 1f)
    {
        if (clip == null || SFXVolume <= 0.01f) return;

        var src = audioPool[audioIndex];
        audioIndex = (audioIndex + 1) % audioPool.Length;

        src.transform.position = pos;
        src.volume = Mathf.Clamp01(SFXVolume * volume);
        src.pitch = pitch;
        src.clip = clip;
        src.Play();
    }
}