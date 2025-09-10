using UnityEngine;

public class HealthOrbIndicator : MonoBehaviour
{
    public Transform player;
    public Transform arrowIndicator;
    public string healthOrbTag = "HealthOrb";

    public float radius = 2f; // Player etrafındaki halka yarıçapı

    void Update()
    {
        GameObject[] orbs = GameObject.FindGameObjectsWithTag(healthOrbTag);
        if (orbs.Length == 0)
        {
            arrowIndicator.gameObject.SetActive(false);
            return;
        }

        GameObject nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (var orb in orbs)
        {
            float dist = Vector3.Distance(player.position, orb.transform.position);
            if (dist < nearestDist)
            {
                nearest = orb;
                nearestDist = dist;
            }
        }

        if (nearest != null)
        {
            arrowIndicator.gameObject.SetActive(true);

            Vector3 dir = (nearest.transform.position - player.position).normalized;
            dir.y = 0f;

            // Okun player etrafındaki pozisyonu
            Vector3 indicatorPos = player.position + dir * radius;
            indicatorPos.y = 0.01f; // Zemin hizasında tut

            arrowIndicator.position = indicatorPos;

            Quaternion rot = Quaternion.LookRotation(dir);
            arrowIndicator.rotation = Quaternion.Euler(90f, rot.eulerAngles.y, 0f); // X sabit 90
        }
        else
        {
            arrowIndicator.gameObject.SetActive(false);
        }
    }
}
