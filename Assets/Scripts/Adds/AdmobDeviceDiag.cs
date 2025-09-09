// Assets/Scripts/Ads/AdmobDeviceDiag.cs
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;
using AdRequest = GoogleMobileAds.Api.AdRequest;

public class AdmobDeviceDiag : MonoBehaviour
{
    [Header("TEST Interstitial ID")]
    public string interstitialID = "ca-app-pub-3940256099942544/1033173712";

    void Start()
    {
        Debug.Log("[GMA] HasGooglePlayServices=" + HasGooglePlayServices());
        MobileAds.Initialize(_ =>
        {
            Debug.Log("[GMA] Initialized");
            LoadAndShowOnce();
        });
    }

    void LoadAndShowOnce()
    {
        var req = new AdRequest(); // Unity için doğru kullanım
        Debug.Log("[GMA] Loading interstitial...");
        InterstitialAd.Load(
            interstitialID,
            req,
            (InterstitialAd ad, LoadAdError err) =>
            {
                if (err != null || ad == null)
                {
                    Debug.LogError(
                        "[GMA] LOAD FAIL: code="
                            + err.GetCode()
                            + " domain="
                            + err.GetDomain()
                            + " msg="
                            + err.GetMessage()
                    );
                    try
                    {
                        Debug.Log("[GMA] ResponseInfo: " + err.GetResponseInfo());
                    }
                    catch { }
                    return;
                }

                try
                {
                    Debug.Log("[GMA] LOADED. ResponseInfo: " + ad.GetResponseInfo());
                }
                catch { }
                ad.OnAdFullScreenContentFailed += (AdError e) =>
                    Debug.LogError(
                        "[GMA] SHOW FAIL: "
                            + e.GetDomain()
                            + " / "
                            + e.GetCode()
                            + " / "
                            + e.GetMessage()
                    );
                ad.OnAdFullScreenContentClosed += () => Debug.Log("[GMA] CLOSED");
                ad.Show();
            }
        );
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    static bool HasGooglePlayServices()
    {
        try
        {
            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using var apiClass = new AndroidJavaClass(
                "com.google.android.gms.common.GoogleApiAvailability"
            );
            var api = apiClass.CallStatic<AndroidJavaObject>("getInstance");
            int result = api.Call<int>("isGooglePlayServicesAvailable", activity);
            return result == 0; // SUCCESS
        }
        catch (Exception e)
        {
            Debug.Log("[GMA] GMS check failed: " + e.Message);
            return true; // emin değilsek engelleme
        }
    }
#else
    static bool HasGooglePlayServices() => true;
#endif
}
