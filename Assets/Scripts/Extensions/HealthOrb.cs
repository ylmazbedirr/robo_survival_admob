using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeHeal(50); // Canı 100 yap
            Destroy(gameObject); // Topu yok et
        }
    }
}
