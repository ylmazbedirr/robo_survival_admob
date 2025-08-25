/*

// Assets/Scripts/Gameplay/GoldManager.cs
using UnityEngine;
using TMPro;

/// RUN içi altın sayacı (skor). Bankaya işler, ölümde sıfırlanır.
public class GoldManager : MonoBehaviour
{
    [Header("UI (Run HUD)")]
    [SerializeField] private TMP_Text goldText;

    [Header("Rule")]
    [Min(1)] public int killsPerGold = 20;

    private int _killCount;
    private int _sessionGold;

    private void OnEnable() => UpdateHud();

    public void AddKill()
    {
        // Neden: Servis hazır değilse patlama; Prewarm var ama bu ekstra güvenlik.
        var svc = GoldService.Instance;
        if (svc == null) return;

        _killCount++;
        if (_killCount >= killsPerGold)
        {
            _killCount = 0;
            _sessionGold++;
            svc.AddGold(1);           // bankaya kalıcı işle
            UpdateHud();
        }
    }

    public void ResetRun()
    {
        _killCount = 0;
        _sessionGold = 0;
        UpdateHud();
    }

    private void UpdateHud()
    {
        if (goldText != null) goldText.text = $": {_sessionGold}";
    }
}

*/

using UnityEngine;
using TMPro;


public class GoldManager : MonoBehaviour
{
    [Header("UI (Run HUD)")]
    [SerializeField] private TMP_Text goldText;


    [Header("Rule")]
    [Min(1)] public int killsPerGold = 10;


    private int _killCount;
    private int _sessionGold;


    private void OnEnable() => UpdateHud();


    public void AddKill()
    {
        var svc = GoldService.Instance; if (svc == null) return;
        _killCount++;
        if (_killCount >= killsPerGold)
        {
            _killCount = 0; _sessionGold++; svc.AddGold(1);
            UpdateHud();
        }
    }


    public void ResetRun()
    {
        _killCount = 0; _sessionGold = 0; UpdateHud();
    }


    private void UpdateHud()
    {
        if (goldText != null) goldText.text = $": {_sessionGold}";
    }
}