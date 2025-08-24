/*
// File: Assets/Scripts/UI/DeathMenuTimeUI.cs
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
[DisallowMultipleComponent]
public class DeathMenuTimeUI : MonoBehaviour
{
    private TMP_Text label;
    private const string Prefix = "SCORE: ";

    private void Awake() => label = GetComponent<TMP_Text>();

    private void OnEnable()
    {
        if (label == null) label = GetComponent<TMP_Text>();

        float seconds = 0f;
        var timer = GameTimer.Instance;
        if (timer != null)
        {
            // Daima SON OYUN süresi
            seconds = timer.LastRun > 0f ? timer.LastRun : timer.Elapsed;
        }
        label.text = $"{Prefix}{GameTimer.FormatMMSS(seconds)}";
    }
}

*/

using UnityEngine;
using TMPro;


[RequireComponent(typeof(TMP_Text))]
[DisallowMultipleComponent]
public class DeathMenuTimeUI : MonoBehaviour
{
    private TMP_Text label;
    private const string Prefix = "SCORE: ";


    private void Awake() => label = GetComponent<TMP_Text>();


    private void OnEnable()
    {
        if (label == null) label = GetComponent<TMP_Text>();
        float seconds = 0f;
        var timer = GameTimer.Instance;
        if (timer != null)
        {
            seconds = timer.LastRun > 0f ? timer.LastRun : timer.Elapsed;
        }
        label.text = $"{Prefix}{GameTimer.FormatMMSS(seconds)}";
    }
}