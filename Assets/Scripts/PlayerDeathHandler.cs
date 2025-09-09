/*
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;     // Boş bırakılabilir
    [SerializeField] private GameObject deathMenu;    // Boş bırakılabilir

    private void Awake()
    {
        if (health == null) health = GetComponent<PlayerHealth>();
        if (deathMenu == null)
        {
            // Önce tag ile dene (UI paneline "DeathMenu" tag'i verebilirsin)
            var tagged = GameObject.FindGameObjectWithTag("DeathMenu");
            if (tagged != null) deathMenu = tagged;
            else
            {
                // İsimle dene
                var named = GameObject.Find("DeathMenu");
                if (named != null) deathMenu = named;
            }
        }

        if (deathMenu != null) deathMenu.SetActive(false);
    }

    private void OnEnable()
    {
        if (health != null) health.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        if (health != null) health.OnDied -= HandleDeath;
    }

    private void HandleDeath()
    {
        Time.timeScale = 0f;
        if (deathMenu != null) deathMenu.SetActive(true);
      //  else Debug.LogWarning("DeathMenu paneli bulunamadı.");
    }
}

*/

using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth health;

    [SerializeField]
    private GameObject deathMenu;

    private void Awake()
    {
        if (health == null)
            health = GetComponent<PlayerHealth>();
        if (deathMenu == null)
        {
            var tagged = GameObject.FindGameObjectWithTag("DeathMenu");
            if (tagged != null)
                deathMenu = tagged;
            else
            {
                var named = GameObject.Find("DeathMenu");
                if (named != null)
                    deathMenu = named;
            }
        }
        if (deathMenu != null)
            deathMenu.SetActive(false);
    }

    private void OnEnable()
    {
        if (health != null)
            health.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnDied -= HandleDeath;
    }

    private void HandleDeath()
    {
        Time.timeScale = 0f;
        if (deathMenu != null)
            deathMenu.SetActive(true);
    }
}
