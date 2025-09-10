using UnityEngine;

public class RedZone : MonoBehaviour
{
    [SerializeField] private int damagePerSecond = 4;

    private float damageTimer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                damageTimer += Time.deltaTime;

                if (damageTimer >= 0.2f)
                {
                    player.TakeDamage(damagePerSecond);
                    damageTimer = 0f;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // RedZone'dan çıkınca zamanlayıcıyı sıfırla
            damageTimer = 0f;
        }
    }
}
