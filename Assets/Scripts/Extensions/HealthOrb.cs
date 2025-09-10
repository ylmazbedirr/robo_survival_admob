using UnityEngine;

<<<<<<< HEAD
public class HealthOrb : MonoBehaviour
=======
public class HealthOrB : MonoBehaviour
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35
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
