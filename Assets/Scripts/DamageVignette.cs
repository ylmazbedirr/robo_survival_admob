/*

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageVignette : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth; // PlayerHealth script’in
    [SerializeField] private Image vignetteImage;       // UI Image referansı
    [SerializeField] private float flashAlpha = 0.35f;  // Hasar anındaki maksimum alpha
    [SerializeField] private float fadeSpeed = 2.5f;    // Ne kadar hızlı kaybolacak

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (vignetteImage != null)
            vignetteImage.canvasRenderer.SetAlpha(0f);

        // PlayerHealth’i otomatik bul
        if (playerHealth == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player) playerHealth = player.GetComponent<PlayerHealth>();
        }

        // PlayerHealth’ten hasar olayına abone ol
        if (playerHealth != null)
            playerHealth.OnDamaged += OnPlayerDamaged;
    }

    private void OnPlayerDamaged(int currentHP)
    {
        // Alpha’yı hasar efekti seviyesine çek
        vignetteImage.canvasRenderer.SetAlpha(flashAlpha);

        // Eğer fade coroutine çalışıyorsa durdur
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = flashAlpha;
        while (alpha > 0f)
        {
            alpha -= fadeSpeed * Time.unscaledDeltaTime; // pause olsa bile çalışsın
            vignetteImage.canvasRenderer.SetAlpha(Mathf.Max(0f, alpha));
            yield return null;
        }
    }
}

*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageVignette : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image vignetteImage;
    [SerializeField] private float flashAlpha = 0.35f;
    [SerializeField] private float fadeSpeed = 2.5f;


    private Coroutine fadeCoroutine;


    void Awake()
    {
        if (vignetteImage != null) vignetteImage.canvasRenderer.SetAlpha(0f);
        if (playerHealth == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerHealth = p.GetComponent<PlayerHealth>();
        }
        if (playerHealth != null) playerHealth.OnDamaged += OnPlayerDamaged;
    }

    private void OnPlayerDamaged(int currentHP)
    {
        if (vignetteImage == null) return;
        vignetteImage.canvasRenderer.SetAlpha(flashAlpha);
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        float alpha = flashAlpha;
        while (alpha > 0f)
        {
            alpha -= fadeSpeed * Time.unscaledDeltaTime;
            if (vignetteImage != null)
                vignetteImage.canvasRenderer.SetAlpha(Mathf.Max(0f, alpha));
            yield return null;
        }
    }
}

