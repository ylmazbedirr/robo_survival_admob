using System;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class InterstitialFrequencyPolicy : MonoBehaviour
{
    [Header("Frequency")]
    [Tooltip("Kaçıncı tıklamada bir gösterilsin? Örn: 3 = her 3. tıklama")]
    [Min(1)]
    public int showEveryNth = 3;

    [Header("Cooldown")]
    [Tooltip("Son reklamdan sonra minimum bekleme süresi (saniye)")]
    [Min(0)]
    public int cooldownSeconds = 45;

    [Header("Persistence")]
    [Tooltip("Sayaç ve son gösterim zamanı oturumlar arasında saklansın mı?")]
    public bool persistAcrossSessions = true;

    [SerializeField, Tooltip("Aynı cihazda birden fazla oyun/versiyon varsa anahtar ayırmak için.")]
    private string prefsKeyPrefix = "ads_interstitial";

    int _retryCountSession;
    DateTime _lastShownUtc;

    string KeyCount => $"{prefsKeyPrefix}_retry_count";
    string KeyLast => $"{prefsKeyPrefix}_last_shown_ticks";

    void Awake()
    {
        if (persistAcrossSessions)
        {
            _retryCountSession = PlayerPrefs.GetInt(KeyCount, 0);
            long ticks = 0;
            long.TryParse(PlayerPrefs.GetString(KeyLast, "0"), out ticks);
            _lastShownUtc = ticks > 0 ? new DateTime(ticks, DateTimeKind.Utc) : DateTime.MinValue;
        }
        else
        {
            _retryCountSession = 0;
            _lastShownUtc = DateTime.MinValue;
        }
    }

    /// <summary>
    /// WHY: Retry tıklanınca çağır. Sayaç artar, cooldown uygular, True dönerse reklam gösterilebilir.
    /// </summary>
    public bool ShouldShowNowAndBump()
    {
        // Cooldown kontrolü (neden: sık spam engelle)
        var since = (DateTime.UtcNow - _lastShownUtc).TotalSeconds;
        bool cooldownOk = since >= cooldownSeconds;

        // Sayaç artır (neden: tıklama bazlı frekans)
        _retryCountSession++;
        bool nthOk = (_retryCountSession % Mathf.Max(1, showEveryNth)) == 0;

        bool allow = cooldownOk && nthOk;

        if (persistAcrossSessions)
        {
            PlayerPrefs.SetInt(KeyCount, _retryCountSession);
            PlayerPrefs.Save();
        }

        return allow;
    }

    /// <summary>
    /// WHY: Reklam gerçekten gösterildiğinde çağır. Cooldown başlangıcı buradan başlar.
    /// </summary>
    public void MarkShownNow()
    {
        _lastShownUtc = DateTime.UtcNow;
        if (persistAcrossSessions)
        {
            PlayerPrefs.SetString(KeyLast, _lastShownUtc.Ticks.ToString());
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Opsiyonel yardımcı: Sayaçları sıfırla (debug/test için).
    /// </summary>
    public void ResetPolicy()
    {
        _retryCountSession = 0;
        _lastShownUtc = DateTime.MinValue;
        if (persistAcrossSessions)
        {
            PlayerPrefs.DeleteKey(KeyCount);
            PlayerPrefs.DeleteKey(KeyLast);
        }
    }
}
