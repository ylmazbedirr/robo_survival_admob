/*

using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsText;
    private float deltaTime;


    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }
}

*/

using UnityEngine;
using UnityEngine.UI;


public class FPSCounter : MonoBehaviour
{
    public Text fpsText;
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / Mathf.Max(0.000001f, deltaTime);
        if (fpsText != null) fpsText.text = "FPS: " + Mathf.CeilToInt(fps).ToString();
    }
}
