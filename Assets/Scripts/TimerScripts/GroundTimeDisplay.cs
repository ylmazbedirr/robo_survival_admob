/*
// File: Assets/Scripts/UI/GroundTimeDisplay.cs
using UnityEngine;
using TMPro;

public class GroundTimeDisplay : MonoBehaviour
{
    private const string Prefix = "Score: ";

    [Header("Refs")]
    [SerializeField] private Transform player;     // Zeminde konumlamak için
    [SerializeField] private TMP_Text text3D;      // 3D TMP (UGUI değil)
    [SerializeField] private LayerMask groundMask; // Zeminin Layer'ı

    [Header("Placement")]
    [SerializeField] private float rayHeight = 5f;
    [SerializeField] private float hover = 0.03f;
    [SerializeField] private float followLerp = 20f;

    private Camera cam;
    private bool subscribed;

    private void Awake()
    {
        cam = Camera.main;
        if (!text3D) text3D = GetComponent<TMP_Text>();

        TrySubscribeTimer();
        if (!subscribed && text3D) text3D.text = $"{Prefix}00:00";
    }

    private void OnEnable()
    {
        // Sahne sırası/geç yükleme durumlarına karşı güvence
        if (!subscribed) TrySubscribeTimer();
        if (!subscribed && text3D) text3D.text = $"{Prefix}00:00";
    }

    private void OnDestroy()
    {
        if (GameTimer.Instance != null && subscribed)
        {
            GameTimer.Instance.OnTick -= OnTick;
            subscribed = false;
        }
    }

    private void LateUpdate()
    {
        if (!player || !text3D) return;

        Vector3 origin = player.position + Vector3.up * rayHeight;
        if (Physics.Raycast(origin, Vector3.down, out var hit, rayHeight * 2f, groundMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 targetPos = hit.point + hit.normal * hover;
            transform.position = Vector3.Lerp(transform.position, targetPos, followLerp * Time.unscaledDeltaTime);

            if (cam == null) cam = Camera.main;
            Vector3 camFwdOnPlane = Vector3.ProjectOnPlane((cam ? cam.transform.forward : Vector3.forward), hit.normal);
            if (camFwdOnPlane.sqrMagnitude < 0.0001f) camFwdOnPlane = Vector3.forward;
            transform.rotation = Quaternion.LookRotation(camFwdOnPlane, hit.normal);
        }
    }

    private void OnTick(float seconds)
    {
        if (text3D) text3D.text = $"{Prefix}{GameTimer.FormatMMSS(seconds)}";
    }

    private void TrySubscribeTimer()
    {
        if (GameTimer.Instance != null && !subscribed)
        {
            GameTimer.Instance.OnTick += OnTick;
            subscribed = true;
            // İlk frame yazı güncelle
            OnTick(GameTimer.Instance.Elapsed);
        }
    }
}

*/

using UnityEngine;
using TMPro;

public class GroundTimeDisplay : MonoBehaviour
{
    private const string Prefix = "Score: ";


    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private TMP_Text text3D;
    [SerializeField] private LayerMask groundMask;


    [Header("Placement")]
    [SerializeField] private float rayHeight = 5f;
    [SerializeField] private float hover = 0.03f;
    [SerializeField] private float followLerp = 20f;


    private Camera cam; private bool subscribed;


    private void Awake()
    {
        cam = Camera.main; if (!text3D) text3D = GetComponent<TMP_Text>();
        TrySubscribeTimer(); if (!subscribed && text3D) text3D.text = $"{Prefix}00:00";
    }

    private void OnEnable()
    {
        if (!subscribed) TrySubscribeTimer();
        if (!subscribed && text3D) text3D.text = $"{Prefix}00:00";
    }


    private void OnDestroy()
    {
        if (GameTimer.Instance != null && subscribed)
        { GameTimer.Instance.OnTick -= OnTick; subscribed = false; }
    }


    private void LateUpdate()
    {
        if (!player || !text3D) return;
        Vector3 origin = player.position + Vector3.up * rayHeight;
        if (Physics.Raycast(origin, Vector3.down, out var hit, rayHeight * 2f, groundMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 targetPos = hit.point + hit.normal * hover;
            transform.position = Vector3.Lerp(transform.position, targetPos, followLerp * Time.unscaledDeltaTime);
            if (cam == null) cam = Camera.main;
            Vector3 camFwdOnPlane = Vector3.ProjectOnPlane((cam ? cam.transform.forward : Vector3.forward), hit.normal);
            if (camFwdOnPlane.sqrMagnitude < 0.0001f) camFwdOnPlane = Vector3.forward;
            transform.rotation = Quaternion.LookRotation(camFwdOnPlane, hit.normal);
        }
    }

    private void OnTick(float seconds)
    { if (text3D) text3D.text = $"{Prefix}{GameTimer.FormatMMSS(seconds)}"; }


    private void TrySubscribeTimer()
    {
        if (GameTimer.Instance != null && !subscribed)
        {
            GameTimer.Instance.OnTick += OnTick; subscribed = true; OnTick(GameTimer.Instance.Elapsed);
        }
    }
}
