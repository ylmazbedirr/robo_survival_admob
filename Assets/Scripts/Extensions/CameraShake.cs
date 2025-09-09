using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private CinemachineImpulseSource impulse;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        impulse = GetComponent<CinemachineImpulseSource>();
        if (impulse == null)
            impulse = gameObject.AddComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        impulse.GenerateImpulse(intensity);
    }
}
