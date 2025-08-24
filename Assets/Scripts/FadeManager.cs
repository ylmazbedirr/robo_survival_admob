/*
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeSpeed = 1f;

    private void Awake()
    {
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = 0; // Başlangıçta görünmez
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1));
    }

    private System.Collections.IEnumerator FadeRoutine(float targetAlpha)
    {
        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(
                fadeCanvasGroup.alpha,
                targetAlpha,
                fadeSpeed * Time.unscaledDeltaTime // timeScale etkilenmesin
            );
            yield return null;
        }
    }
}

*/

using UnityEngine;


public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeSpeed = 1f;


    private void Awake()
    {
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0;
    }


    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1));
    }


    private System.Collections.IEnumerator FadeRoutine(float targetAlpha)
    {
        while (fadeCanvasGroup != null && !Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
    }
}
