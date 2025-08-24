/*

// File: Assets/Scripts/SplashScreen.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[DisallowMultipleComponent]
public class SplashScreen : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CanvasGroup canvasGroup;   // SplashRoot (parent) üzerinde
    [SerializeField] private string nextScene = "MainMenu";

    [Header("Timings (seconds)")]
    private float fadeIn = 0.8f;
    private float hold = 0.8f;
    private float fadeOut = 0.5f;

    private void Awake()
    {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup)
        {
            Debug.LogError("SplashScreen: CanvasGroup atanmamış!");
            enabled = false;
            return;
        }

        canvasGroup.alpha = 0f;            // başta görünmez
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true; // altta tıklamayı kes
    }

    private void Start()
    {
        StartCoroutine(CoRun());
    }

    private IEnumerator CoRun()
    {
        // 1) Sonraki sahneyi arka planda yükle (aktivasyon kapalı)
        var op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        // 2) Fade In
        yield return Fade(0f, 1f, fadeIn);

        // 3) Logo ekranda kalsın
        yield return WaitRealtime(hold);

        // 4) Sahne %90 hazır olana dek bekle (Unity 0.9'da takılı kalır, aktive edene kadar)
        while (op.progress < 0.9f)
            yield return null;

        // 5) Fade Out (artık yeni sahne hazır; fade bitince "anında" geçeceğiz)
        yield return Fade(1f, 0f, fadeOut);

        // 6) Tek frame sonra aktive et (gerçekten anlık geçiş)
        yield return null;
        op.allowSceneActivation = true;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            canvasGroup.alpha = to;
            yield break;
        }

        float t = 0f;
        canvasGroup.alpha = from;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    private IEnumerator WaitRealtime(float seconds)
    {
        float t = 0f;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}

*/


using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


[DisallowMultipleComponent]
public class SplashScreen : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private string nextScene = "MainMenu";


    [Header("Timings (seconds)")]
    private float fadeIn = 0.8f; private float hold = 0.8f; private float fadeOut = 0.5f;


    private void Awake()
    {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup) { Debug.LogError("SplashScreen: CanvasGroup atanmamış!"); enabled = false; return; }
        canvasGroup.alpha = 0f; canvasGroup.interactable = false; canvasGroup.blocksRaycasts = true;
    }

    private void Start() { StartCoroutine(CoRun()); }


    private IEnumerator CoRun()
    {
        var op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
        op.allowSceneActivation = false;
        yield return Fade(0f, 1f, fadeIn);
        yield return WaitRealtime(hold);
        while (op.progress < 0.9f) yield return null;
        yield return Fade(1f, 0f, fadeOut);
        yield return null; op.allowSceneActivation = true;
    }


    private IEnumerator Fade(float from, float to, float duration)
    {
        if (duration <= 0f) { canvasGroup.alpha = to; yield break; }
        float t = 0f; canvasGroup.alpha = from;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; canvasGroup.alpha = Mathf.Lerp(from, to, t / duration); yield return null;
        }
        canvasGroup.alpha = to;
    }


    private IEnumerator WaitRealtime(float seconds)
    {
        float t = 0f; while (t < seconds) { t += Time.unscaledDeltaTime; yield return null; }
    }
}