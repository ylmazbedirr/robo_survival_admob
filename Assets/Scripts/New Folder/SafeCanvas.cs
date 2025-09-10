/* 
// File: Assets/Scripts/UI/SafeCanvas.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Canvas üzerinde güvenli alanı otomatik uygular.
/// - SafeAreaRoot adlı bir RectTransform oluşturur
/// - (İsteğe bağlı) mevcut tüm çocukları oraya taşır
/// - SafeAreaIgnore olanları taşımadan bırakır
/// - Çözünürlük/orientation değişiminde kendini günceller
[ExecuteAlways, RequireComponent(typeof(Canvas))]
public class SafeCanvas : MonoBehaviour
{
    [Header("Edges to protect")]
    public bool protectTop = true;
    public bool protectBottom = true;
    public bool protectLeft = true;
    public bool protectRight = true;

    [Header("One-time setup")]
    [Tooltip("Açıkken, Canvas altındaki tüm çocukları SafeAreaRoot altına taşır. SafeAreaIgnore olanlara dokunmaz.")]
    public bool wrapExistingChildren = true;

    [Header("Extra padding (px)")]
    public int extraLeft, extraRight, extraTop, extraBottom;

    [Header("Debug")]
    public bool showDebugOverlay = false;
    public Color debugColor = new(0f, 1f, 0f, 0.12f);

#if UNITY_EDITOR
    [Header("Editor Simulation")]
    public bool simulateInEditor = false;
    public Rect editorSafeAreaPx = new Rect(0, 80, 1080, 2000);
#endif

    const string RootName = "SafeAreaRoot";
    RectTransform _root;       // SafeAreaRoot
    RectTransform _canvasRT;   // Canvas'ın RT'si

    Rect _lastSafe;
    Vector2Int _lastScreen;

    void OnEnable()
    {
        EnsureHierarchy();
        ApplyNow(true);
    }

    void Awake()
    {
        EnsureHierarchy();
        ApplyNow(true);
    }

    void Update()
    {
        var scr = new Vector2Int(Screen.width, Screen.height);
        var sa = GetSafeArea();

        if (scr != _lastScreen || sa != _lastSafe)
            ApplyNow();
    }

    void EnsureHierarchy()
    {
        if (_canvasRT == null) _canvasRT = GetComponent<RectTransform>();

        // SafeAreaRoot var mı?
        var t = transform.Find(RootName);
        if (t == null)
        {
            var go = new GameObject(RootName, typeof(RectTransform));
            go.transform.SetParent(transform, false);
            _root = go.GetComponent<RectTransform>();
            _root.anchorMin = Vector2.zero;
            _root.anchorMax = Vector2.one;
            _root.offsetMin = Vector2.zero;
            _root.offsetMax = Vector2.zero;
            _root.pivot = new Vector2(0.5f, 0.5f);

            // ilk kez kuruluyorsa çocukları taşı
            if (wrapExistingChildren)
            {
                MoveChildrenUnderRoot();
            }

            // debug overlay
            if (showDebugOverlay)
                CreateOrUpdateDebugOverlay();
        }
        else
        {
            _root = t as RectTransform;
            if (showDebugOverlay)
                CreateOrUpdateDebugOverlay();
            else
                RemoveDebugOverlay();
        }
    }

    void MoveChildrenUnderRoot()
    {
        var toMove = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.name == RootName) continue; // kökü pas geç
            if (child.GetComponent<SafeAreaIgnore>()) continue; // tam ekranlar hariç
            toMove.Add(child);
        }

        foreach (var c in toMove)
            c.SetParent(_root, true);
    }

    Rect GetSafeArea()
    {
#if UNITY_EDITOR
        if (simulateInEditor) return editorSafeAreaPx;
#endif
        return Screen.safeArea;
    }

    void ApplyNow(bool forceRecalcChildren = false)
    {
        if (_root == null) return;

        _lastScreen = new Vector2Int(Screen.width, Screen.height);
        _lastSafe = GetSafeArea();

        // Ekran kenarlarından emniyetsiz bölgeler (px)
        float left = _lastSafe.xMin;
        float right = Screen.width - _lastSafe.xMax;
        float bottom = _lastSafe.yMin;
        float top = Screen.height - _lastSafe.yMax;

        // Hangi kenarlar korunacak?
        float offLeft = (protectLeft ? left : 0f) + extraLeft;
        float offRight = (protectRight ? right : 0f) + extraRight;
        float offBottom = (protectBottom ? bottom : 0f) + extraBottom;
        float offTop = (protectTop ? top : 0f) + extraTop;

        // SafeAreaRoot: Tam ekran anchor + offsetlerle inset
        _root.anchorMin = Vector2.zero;
        _root.anchorMax = Vector2.one;
        _root.pivot = new Vector2(0.5f, 0.5f);
        _root.offsetMin = new Vector2(offLeft, offBottom);
        _root.offsetMax = new Vector2(-offRight, -offTop);

        if (showDebugOverlay) CreateOrUpdateDebugOverlay();
    }

    // Debug overlay: SafeAreaRoot içine yarı saydam Image
    const string DebugName = "__SafeDebug";
    void CreateOrUpdateDebugOverlay()
    {
        var ex = _root.Find(DebugName);
        Image img;
        if (ex == null)
        {
            var go = new GameObject(DebugName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(_root, false);
            img = go.GetComponent<Image>();
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
        else
        {
            img = ex.GetComponent<Image>();
        }
        img.color = debugColor;
        img.raycastTarget = false;
    }

    void RemoveDebugOverlay()
    {
        var ex = _root ? _root.Find(DebugName) : null;
        if (ex != null) DestroyImmediate(ex.gameObject);
    }
}

*/


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways, RequireComponent(typeof(Canvas))]
public class SafeCanvas : MonoBehaviour
{
    [Header("Edges to protect")] public bool protectTop = true, protectBottom = true, protectLeft = true, protectRight = true;
    [Header("One-time setup")] public bool wrapExistingChildren = true;
    [Header("Extra padding (px)")] public int extraLeft, extraRight, extraTop, extraBottom;
    [Header("Debug")] public bool showDebugOverlay = false; public Color debugColor = new Color(0f, 1f, 0f, 0.12f);
#if UNITY_EDITOR
    [Header("Editor Simulation")] public bool simulateInEditor = false; public Rect editorSafeAreaPx = new Rect(0, 80, 1080, 2000);
#endif
    const string RootName = "SafeAreaRoot"; const string DebugName = "__SafeDebug";
    RectTransform _root; RectTransform _canvasRT; Rect _lastSafe; Vector2Int _lastScreen;


    void OnEnable() { EnsureHierarchy(); ApplyNow(true); }
    void Awake() { EnsureHierarchy(); ApplyNow(true); }


    void Update()
    {
        var scr = new Vector2Int(Screen.width, Screen.height); var sa = GetSafeArea();
        if (scr != _lastScreen || sa != _lastSafe) ApplyNow();
    }


    void EnsureHierarchy()
    {
        if (_canvasRT == null) _canvasRT = GetComponent<RectTransform>();
        var t = transform.Find(RootName);
        if (t == null)
        {
            var go = new GameObject(RootName, typeof(RectTransform)); go.transform.SetParent(transform, false);
            _root = go.GetComponent<RectTransform>(); _root.anchorMin = Vector2.zero; _root.anchorMax = Vector2.one; _root.offsetMin = Vector2.zero; _root.offsetMax = Vector2.zero; _root.pivot = new Vector2(0.5f, 0.5f);
            if (wrapExistingChildren) MoveChildrenUnderRoot();
            if (showDebugOverlay) CreateOrUpdateDebugOverlay();
        }
        else
        {
            _root = t as RectTransform; if (showDebugOverlay) CreateOrUpdateDebugOverlay(); else RemoveDebugOverlay();
        }
    }
    void MoveChildrenUnderRoot()
    {
        var toMove = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.name == RootName) continue;
            if (child.GetComponent<SafeAreaIgnore>()) continue;
            toMove.Add(child);
        }
        foreach (var c in toMove) c.SetParent(_root, true);
    }


    Rect GetSafeArea()
    {
#if UNITY_EDITOR
        if (simulateInEditor) return editorSafeAreaPx;
#endif
        return Screen.safeArea;
    }


    void ApplyNow(bool forceRecalcChildren = false)
    {
        if (_root == null) return;
        _lastScreen = new Vector2Int(Screen.width, Screen.height); _lastSafe = GetSafeArea();
        float left = _lastSafe.xMin; float right = Screen.width - _lastSafe.xMax; float bottom = _lastSafe.yMin; float top = Screen.height - _lastSafe.yMax;
        float offLeft = (protectLeft ? left : 0f) + extraLeft;
        float offRight = (protectRight ? right : 0f) + extraRight;
        float offBottom = (protectBottom ? bottom : 0f) + extraBottom;
        float offTop = (protectTop ? top : 0f) + extraTop;
        _root.anchorMin = Vector2.zero; _root.anchorMax = Vector2.one; _root.pivot = new Vector2(0.5f, 0.5f);
        _root.offsetMin = new Vector2(offLeft, offBottom); _root.offsetMax = new Vector2(-offRight, -offTop);
        if (showDebugOverlay) CreateOrUpdateDebugOverlay();
    }


    void CreateOrUpdateDebugOverlay()
    {
        var ex = _root.Find(DebugName); Image img;
        if (ex == null)
        {
            var go = new GameObject(DebugName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(_root, false); img = go.GetComponent<Image>(); var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }
        else { img = ex.GetComponent<Image>(); }
        img.color = debugColor; img.raycastTarget = false;
    }


    void RemoveDebugOverlay()
    {
        var ex = _root ? _root.Find(DebugName) : null; if (ex != null) DestroyImmediate(ex.gameObject);
    }
}