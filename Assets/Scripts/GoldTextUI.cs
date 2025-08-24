/*

// Assets/Scripts/UI/GoldTextUI.cs
// Neden: Ana menü (veya istersen HUD) toplam kalıcı altını otomatik göstersin.
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public sealed class GoldTextUI : MonoBehaviour
{
    [SerializeField] private string prefix = " ";
    private TMP_Text _txt;
    private GoldService _svc;

    private void Awake() { _txt = GetComponent<TMP_Text>(); }
    private void OnEnable()
    {
        _svc = GoldService.Instance;
        if (_svc == null) return;
        _txt.text = prefix + _svc.TotalGold;
        _svc.OnGoldChanged += OnGoldChanged;
    }
    private void OnDisable()
    {
        if (_svc != null) _svc.OnGoldChanged -= OnGoldChanged;
        _svc = null;
    }
    private void OnGoldChanged(int total) => _txt.text = prefix + total;
}

*/

using UnityEngine;
using TMPro;


[RequireComponent(typeof(TMP_Text))]
public sealed class GoldTextUI : MonoBehaviour
{
    [SerializeField] private string prefix = " ";
    private TMP_Text _txt; private GoldService _svc;


    private void Awake() { _txt = GetComponent<TMP_Text>(); }
    private void OnEnable()
    {
        _svc = GoldService.Instance; if (_svc == null) return;
        _txt.text = prefix + _svc.TotalGold;
        _svc.OnGoldChanged += OnGoldChanged;
    }
    private void OnDisable()
    {
        if (_svc != null) _svc.OnGoldChanged -= OnGoldChanged; _svc = null;
    }
    private void OnGoldChanged(int total) => _txt.text = prefix + total;
}

