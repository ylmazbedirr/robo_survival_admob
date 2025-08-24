/* using UnityEngine;

public class FPSdeneme : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        QualitySettings.vSyncCount = 0;           // VSync kapalı (aksi halde cihazın pacing’i yönetir)
        Application.targetFrameRate = 60;          // Stabil 60 FPS
        // Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

*/



using UnityEngine;


public class FPSdeneme : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;
    [SerializeField] private bool disableVSync = true;


    void Awake()
    {
        if (disableVSync) QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }
}
