/*
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;
using AdRequest = GoogleMobileAds.Api.AdRequest;

public class AdmobInterstitial : MonoBehaviour
{
    public static AdmobInterstitial Instance { get; private set; }

    [Header("Interstitial Ad Unit ID")]
    public string interstitialID = "ca-app-pub-3940256099942544/1033173712";

    private InterstitialAd _interstitialAd;
    private Action _pendingOnClosed;
    private bool _initialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // WHY: Tek kopya
    }

    private void Start()
    {
        // Init'i AdsBootstrapUMP_Safe çağıracak; yoksa yine de emniyet olsun
        if (!_initialized)
        {
            MobileAds.Initialize(_ =>
            {
                _initialized = true;
                LoadInterstitial();
            });
        }
    }

    public static AdmobInterstitial Ensure()
    {
        if (Instance != null)
            return Instance;
        var found = FindObjectOfType<AdmobInterstitial>();
        if (found != null)
        {
            Instance = found;
            DontDestroyOnLoad(found.gameObject);
            return Instance;
        }
        var go = new GameObject("AdsManager");
        return Instance = go.AddComponent<AdmobInterstitial>();
    }

    public bool IsReady => _interstitialAd != null && _interstitialAd.CanShowAd();

    public void Preload()
    {
        if (_initialized)
            LoadInterstitial();
    }

    public void ShowInterstitialAd() => ShowInterstitialAdThen(null);

    public void ShowInterstitialAdThen(Action onClosed)
    {
        _pendingOnClosed = onClosed;
        if (IsReady)
            _interstitialAd.Show();
        else
        {
            SafeInvokePending();
            LoadInterstitial();
        }
    }

    private void LoadInterstitial()
    {
        if (!_initialized)
            return;

        _interstitialAd?.Destroy();
        _interstitialAd = null;

        var req = new AdRequest(); // Unity için doğru
        InterstitialAd.Load(
            interstitialID,
            req,
            (InterstitialAd ad, LoadAdError err) =>
            {
                if (err != null || ad == null)
                {
                    Debug.LogWarning(
                        $"[Ads] Load fail: code={err?.GetCode()} domain={err?.GetDomain()} msg={err?.GetMessage()}"
                    );
                    return;
                }

                _interstitialAd = ad;
                RegisterEvents(_interstitialAd);
            }
        );
    }

    private void RegisterEvents(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            SafeInvokePending();
            LoadInterstitial();
        };
        ad.OnAdFullScreenContentFailed += (AdError e) =>
        {
            Debug.LogWarning(
                $"[Ads] Show fail: {e.GetDomain()} / {e.GetCode()} / {e.GetMessage()}"
            );
            SafeInvokePending();
            LoadInterstitial();
        };
    }

    private void SafeInvokePending()
    {
        var cb = _pendingOnClosed;
        _pendingOnClosed = null;
        try
        {
            cb?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}

*/

using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;
using AdRequest = GoogleMobileAds.Api.AdRequest;

public class AdmobInterstitial : MonoBehaviour
{
    public static AdmobInterstitial Instance { get; private set; }

    [Header("Interstitial Ad Unit ID")]
    public string interstitialID = "ca-app-pub-3940256099942544/1033173712"; // TEST

    private InterstitialAd _interstitialAd;
    private Action _pendingOnClosed;
    private bool _initialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (_initialized)
            return;
        MobileAds.Initialize(_ =>
        {
            _initialized = true;
            LoadInterstitial();
        });
    }

    public static AdmobInterstitial Ensure()
    {
        if (Instance != null)
            return Instance;
        var found = FindObjectOfType<AdmobInterstitial>();
        if (found != null)
        {
            Instance = found;
            DontDestroyOnLoad(found.gameObject);
            return Instance;
        }
        var go = new GameObject("AdsManager");
        return Instance = go.AddComponent<AdmobInterstitial>();
    }

    public bool IsReady => _interstitialAd != null && _interstitialAd.CanShowAd();

    public void Preload()
    {
        if (_initialized)
            LoadInterstitial();
    }

    public void ShowInterstitialAd() => ShowInterstitialAdThen(null);

    public void ShowInterstitialAdThen(Action onClosed)
    {
        _pendingOnClosed = onClosed;
        if (IsReady)
            _interstitialAd.Show();
        else
        {
            SafeInvokePending();
            LoadInterstitial();
        }
    }

    private void LoadInterstitial()
    {
        if (!_initialized)
            return;
        _interstitialAd?.Destroy();
        _interstitialAd = null;

        var req = new AdRequest();
        InterstitialAd.Load(
            interstitialID,
            req,
            (InterstitialAd ad, LoadAdError err) =>
            {
                if (err != null || ad == null)
                {
                    Debug.LogWarning($"[Ads] Load fail: {err?.GetMessage()}");
                    return;
                }
                _interstitialAd = ad;
                RegisterEvents(_interstitialAd);
            }
        );
    }

    private void RegisterEvents(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            SafeInvokePending();
            LoadInterstitial();
        };
        ad.OnAdFullScreenContentFailed += (AdError e) =>
        {
            Debug.LogWarning(
                $"[Ads] Show fail: {e.GetDomain()} / {e.GetCode()} / {e.GetMessage()}"
            );
            SafeInvokePending();
            LoadInterstitial();
        };
    }

    private void SafeInvokePending()
    {
        var cb = _pendingOnClosed;
        _pendingOnClosed = null;
        try
        {
            cb?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
