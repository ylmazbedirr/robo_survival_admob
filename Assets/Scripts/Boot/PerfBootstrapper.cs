
// ─────────────────────────────────────────────────────────────────────────────
// File: Assets/Scripts/Boot/PerfBootstrapper.cs
// Purpose: Start PerfRescue + AutoQualityTuner at app boot. No FPS logic here.
// ─────────────────────────────────────────────────────────────────────────────
using UnityEngine;
using System;


[DefaultExecutionOrder(-32000)]
public sealed class PerfBootstrapper : MonoBehaviour
{
    [SerializeField] private bool hideInHierarchy = true;


    private void Awake()
    {
        Ensure("PerfRescue");
        Ensure("AutoQualityTuner");
        Debug.Log("[PerfBootstrapper] ensured PerfRescue + AutoQualityTuner");
    }


    private void Ensure(string typeName)
    {
        string goName = "[" + typeName + "]";
        if (GameObject.Find(goName) != null) return;


        var t = FindType(typeName);
        if (t == null)
        {
            Debug.LogWarning($"[PerfBootstrapper] Type not found: {typeName}. Is the script in Assets (not Editor) and compiled?");
            return;
        }


        var go = new GameObject(goName);
        DontDestroyOnLoad(go);
#if UNITY_EDITOR
        if (hideInHierarchy) go.hideFlags = HideFlags.HideInHierarchy;
#endif
        go.AddComponent(t);
    }


    private static Type FindType(string typeName)
    {
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            var t = asm.GetType(typeName, false);
            if (t != null) return t;
            System.Type[] types;
            try { types = asm.GetTypes(); }
            catch { continue; }
            for (int i = 0; i < types.Length; i++)
                if (types[i].Name == typeName) return types[i];
        }
        return null;
    }
}
