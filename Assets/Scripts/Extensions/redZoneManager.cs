using UnityEngine;

public class RedZoneManager : MonoBehaviour
{
    public GameObject redZone;
    public float zoneRadius = 2f; // çap olarak düşünebilirsin
    public Vector2 xBounds = new Vector2(-19.4f, 22.6f);
    public Vector2 zBounds = new Vector2(-25f, 16.5f);

    private float gameTimer = 0f;
    private float cycleTimer = 0f;
    private bool isZoneActive = false;

    private bool hasStarted = false;

    void Update()
    {
        gameTimer += Time.deltaTime;

        if (gameTimer < 15f) return; // oyun başladıktan 25 sn sonra başla

        cycleTimer += Time.deltaTime;

        if (!hasStarted)
        {
            cycleTimer = 0f;
            hasStarted = true;
            isZoneActive = false;
            redZone.SetActive(false);
        }

        if (!isZoneActive && cycleTimer >= 10f)
        {
            ActivateZone();
        }
        else if (isZoneActive && cycleTimer >= 20f)
        {
            DeactivateZone();
        }
    }

    void ActivateZone()
    {
        redZone.SetActive(true);
        redZone.transform.position = GetRandomPosition();
        isZoneActive = true;
    }

    void DeactivateZone()
    {
        redZone.SetActive(false);
        isZoneActive = false;
        cycleTimer = 0f;
    }

    Vector3 GetRandomPosition()
    {
        float randX = Random.Range(xBounds.x, xBounds.y);
        float randZ = Random.Range(zBounds.x, zBounds.y);
        return new Vector3(randX, 0.1f, randZ); // zemine yakın bir y değeri ver
    }
}
