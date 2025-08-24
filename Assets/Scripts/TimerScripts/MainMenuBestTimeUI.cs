
/*

// File: Assets/Scripts/UI/MainMenuBestTimeUI.cs
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class MainMenuBestTimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text bestText;
    private const string Prefix = "BEST SCORE: ";

    private void Reset()
    {
        bestText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        if (!bestText) bestText = GetComponent<TMP_Text>();
        float best = (GameTimer.Instance != null)
            ? GameTimer.Instance.Best
            : PlayerPrefs.GetFloat("BEST_TIME_SECONDS", 0f);
        if (bestText) bestText.text = $"{Prefix}{GameTimer.FormatMMSS(best)}";
    }

#if UNITY_EDITOR
    // Editörde anında önizleme için (opsiyonel)
    private void OnValidate()
    {
        if (!bestText) bestText = GetComponent<TMP_Text>();
        if (bestText != null)
        {
            float best = PlayerPrefs.GetFloat("BEST_TIME_SECONDS", 0f);
            bestText.text = $"{Prefix}{GameTimer.FormatMMSS(best)}";
        }
    }
#endif
}

*/

using UnityEngine;
using TMPro;


[DisallowMultipleComponent]
public class MainMenuBestTimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text bestText;
    private const string Prefix = "BEST SCORE: ";


    private void Reset() { bestText = GetComponent<TMP_Text>(); }


    private void Start()
    {
        if (!bestText) bestText = GetComponent<TMP_Text>();
        float best = (GameTimer.Instance != null) ? GameTimer.Instance.Best : PlayerPrefs.GetFloat("BEST_TIME_SECONDS", 0f);
        if (bestText) bestText.text = $"{Prefix}{GameTimer.FormatMMSS(best)}";
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!bestText) bestText = GetComponent<TMP_Text>();
        if (bestText != null)
        {
            float best = PlayerPrefs.GetFloat("BEST_TIME_SECONDS", 0f);
            bestText.text = $"{Prefix}{GameTimer.FormatMMSS(best)}";
        }
    }
#endif
}