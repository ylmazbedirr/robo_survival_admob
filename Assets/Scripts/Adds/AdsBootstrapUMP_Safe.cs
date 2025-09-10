using System.Collections.Generic;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using UnityEngine;
using AdRequest = GoogleMobileAds.Api.AdRequest;

public class AdsBootstrapUMP_Safe : MonoBehaviour
{
    [Header("TEST Interstitial ID")]
    public string interstitialID = "ca-app-pub-3940256099942544/1033173712";

    private bool _initialized;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 1) UMP'yi tetikle (ama akışı BLOK-LA-MA!)
        var req = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            // Debug için açmak istersen (EEA simülasyonu):
            // ConsentDebugSettings = new ConsentDebugSettings {
            //     DebugGeography = DebugGeography.EEA,
            //     TestDeviceHashedIds = new List<string> { "HASHED_DEVICE_ID" }
            // }
        };

        ConsentInformation.Update(
            req,
            (FormError updateErr) =>
            {
                // 2) Form gerekiyorsa göster; gerekmiyorsa fast-path
                ConsentForm.LoadAndShowConsentFormIfRequired(
                    (FormError formErr) =>
                    {
                        // 3) Her durumda GMA initialize ET
                        EnsureInitAndPreload();
                    }
                );
            }
        );

        // 4) Ağ/gecikme/UMP hatası olursa diye fail-safe (2 sn sonra yine init et)
        Invoke(nameof(EnsureInitAndPreload), 2f);
    }

    private void EnsureInitAndPreload()
    {
        if (_initialized)
            return;
        MobileAds.Initialize(_ =>
        {
            _initialized = true;
            var ads = AdmobInterstitial.Ensure();
            ads.interstitialID = interstitialID; // test ID
            ads.Preload();
        });
    }

    // Opsiyonel: oyuncuya gizlilik ayarını yeniden açma
    public void OpenPrivacySettings()
    {
        ConsentForm.LoadAndShowConsentFormIfRequired((FormError e) => { });
    }
}
