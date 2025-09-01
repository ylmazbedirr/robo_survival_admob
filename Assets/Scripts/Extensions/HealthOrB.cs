using UnityEngine;

public class HealthOrB : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeHeal(100); // Canı 100 yap
            Destroy(gameObject); // Topu yok et
        }
    }
}
