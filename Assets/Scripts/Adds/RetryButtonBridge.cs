/*

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonBridge : MonoBehaviour
{
    public void OnRetryButton() => StartCoroutine(RetryFlow());

    private IEnumerator RetryFlow()
    {
        Time.timeScale = 1f;
        bool done = false;
        AdmobInterstitial.Ensure().ShowInterstitialAdThen(() => done = true);
        while (!done)
            yield return null;

        Time.timeScale = 1f;
        var idx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(idx);
    }
}
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonBridge : MonoBehaviour
{
    [Header("Policy (opsiyonel)")]
    [SerializeField]
    private InterstitialFrequencyPolicy policy; // Inspector'a verebilirsin. Boşsa runtime'da bulunur/yaratılır.

    [Header("Scene Reload")]
    [SerializeField]
    private bool reloadByName = false;

    [SerializeField]
    private string sceneName = "SampleScene";

    InterstitialFrequencyPolicy Policy
    {
        get
        {
            if (policy != null)
                return policy;
            policy = FindObjectOfType<InterstitialFrequencyPolicy>();
            if (policy == null)
            {
                // WHY: Kullanıcı uğraşmasın; otomatik oluştur.
                var go = new GameObject("InterstitialFrequencyPolicy");
                policy = go.AddComponent<InterstitialFrequencyPolicy>();
            }
            return policy;
        }
    }

    public void OnRetryButton() => StartCoroutine(RetryFlow());

    private IEnumerator RetryFlow()
    {
        // WHY: Donmuş görüntüyü önle
        Time.timeScale = 1f;

        // Frekans kontrolü
        bool shouldShow = Policy.ShouldShowNowAndBump();

        if (shouldShow)
        {
            bool done = false;
            AdmobInterstitial
                .Ensure()
                .ShowInterstitialAdThen(() =>
                {
                    done = true;
                });
            // Reklam kapanana kadar bekle
            while (!done)
                yield return null;

            // Reklam gerçekten gösterildiyse cooldown başlat
            Policy.MarkShownNow();
        }

        // Sahneyi yeniden yükle
        Time.timeScale = 1f;
        if (reloadByName && !string.IsNullOrEmpty(sceneName))
            SceneManager.LoadSceneAsync(sceneName);
        else
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
