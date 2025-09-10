<<<<<<< HEAD
using System;
using System.Collections;
using UnityEngine;
=======
<<<<<<< HEAD
=======
/*

using UnityEngine;
>>>>>>> d039317d713cb09fd8ef2ef750340e470f42d285
using System;
using System.Collections;
using UnityEngine;

>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private int maxHealth = 100;
    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public float Normalized => Mathf.Clamp01((float)CurrentHealth / maxHealth);


    [Header("Death UI")]
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35
    [SerializeField]
    private GameObject deathMenuRoot;

    [SerializeField]
    private CanvasGroup fadeGroup;

    [Header("Death FX")]
    [SerializeField]
    private float slowMoScale = 0.2f;

    [SerializeField]
    private float slowMoHold = 0.6f;

    [SerializeField]
    private float fadeDuration = 0.6f;
<<<<<<< HEAD
=======
=======
    [SerializeField] private GameObject deathMenuRoot;
    [SerializeField] private CanvasGroup fadeGroup;


    [Header("Death FX")]
    [SerializeField] private float slowMoScale = 0.2f;
    [SerializeField] private float slowMoHold = 0.6f;
    [SerializeField] private float fadeDuration = 0.6f;

>>>>>>> d039317d713cb09fd8ef2ef750340e470f42d285
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35

    public event Action<int> OnDamaged;
    public event Action OnDied;


    private bool isDying;

    private void Awake()
    {
        CurrentHealth = maxHealth;
<<<<<<< HEAD
        if (deathMenuRoot)
            deathMenuRoot.SetActive(false);
        if (fadeGroup)
        {
            fadeGroup.alpha = 0f;
            if (!fadeGroup.gameObject.activeSelf)
                fadeGroup.gameObject.SetActive(true);
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
=======
<<<<<<< HEAD
        if (deathMenuRoot)
            deathMenuRoot.SetActive(false);
        if (fadeGroup)
        {
            fadeGroup.alpha = 0f;
            if (!fadeGroup.gameObject.activeSelf)
                fadeGroup.gameObject.SetActive(true);
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
=======
        if (deathMenuRoot) deathMenuRoot.SetActive(false);
        if (fadeGroup)
        {
            fadeGroup.alpha = 0f;
            if (!fadeGroup.gameObject.activeSelf) fadeGroup.gameObject.SetActive(true);
            fadeGroup.blocksRaycasts = false; fadeGroup.interactable = false;
>>>>>>> d039317d713cb09fd8ef2ef750340e470f42d285
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35
        }
    }


    public void TakeDamage(int amount)
    {
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35
        if (amount <= 0 || CurrentHealth <= 0 || isDying)
            return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnDamaged?.Invoke(CurrentHealth);
        if (CurrentHealth == 0 && !isDying)
            StartCoroutine(CoDeathSequence());
=======
        if (amount <= 0 || CurrentHealth <= 0 || isDying) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnDamaged?.Invoke(CurrentHealth);
        if (CurrentHealth == 0 && !isDying) StartCoroutine(CoDeathSequence());
>>>>>>> d039317d713cb09fd8ef2ef750340e470f42d285
    }


    private IEnumerator CoDeathSequence()
    {
        isDying = true;
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35
        if (slowMoScale > 0f)
            Time.timeScale = slowMoScale;

        float tFade = 0f;
        float tHold = slowMoHold;
        if (fadeGroup)
            fadeGroup.blocksRaycasts = true;
<<<<<<< HEAD
=======
=======
        if (slowMoScale > 0f) Time.timeScale = slowMoScale;


        float tFade = 0f; float tHold = slowMoHold;
        if (fadeGroup) fadeGroup.blocksRaycasts = true;
>>>>>>> d039317d713cb09fd8ef2ef750340e470f42d285
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35
        while ((tFade < fadeDuration) || (tHold > 0f))
        {
            if (fadeGroup && tFade < fadeDuration)
            {
                tFade += Time.unscaledDeltaTime;
                fadeGroup.alpha = Mathf.Clamp01(tFade / fadeDuration);
            }
            if (tHold > 0f)
                tHold -= Time.unscaledDeltaTime;
            yield return null;
        }

<<<<<<< HEAD
        if (GameTimer.Instance != null)
            GameTimer.Instance.Stop();
=======
<<<<<<< HEAD
        if (GameTimer.Instance != null)
            GameTimer.Instance.Stop();
=======

        if (GameTimer.Instance != null) GameTimer.Instance.Stop();
        OnDied?.Invoke();
        Time.timeScale = 0f;
        if (deathMenuRoot) deathMenuRoot.SetActive(true);
    }

    public void TakeHeal(int amount)
    {
        if (CurrentHealth <= 0 || isDying) return;
        CurrentHealth = Mathf.Min(maxHealth, amount);
        OnDamaged?.Invoke(CurrentHealth); // UI’ı günceller
    }
}

*/


using UnityEngine;
using System;
using System.Collections;


public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public float Normalized => Mathf.Clamp01((float)CurrentHealth / maxHealth);


    [Header("Death UI")]
    [SerializeField] private GameObject deathMenuRoot;
    [SerializeField] private CanvasGroup fadeGroup;


    [Header("Death FX")]
    [SerializeField] private float slowMoScale = 0.2f;
    [SerializeField] private float slowMoHold = 0.6f;
    [SerializeField] private float fadeDuration = 0.6f;


    public event Action<int> OnDamaged;
    public event Action OnDied;


    private bool isDying;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        if (deathMenuRoot) deathMenuRoot.SetActive(false);
        if (fadeGroup)
        {
            fadeGroup.alpha = 0f;
            if (!fadeGroup.gameObject.activeSelf) fadeGroup.gameObject.SetActive(true);
            fadeGroup.blocksRaycasts = false; fadeGroup.interactable = false;
        }
    }


    public void TakeDamage(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0 || isDying) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnDamaged?.Invoke(CurrentHealth);
        if (CurrentHealth == 0 && !isDying) StartCoroutine(CoDeathSequence());
    }


    private IEnumerator CoDeathSequence()
    {
        isDying = true;
        if (slowMoScale > 0f) Time.timeScale = slowMoScale;


        float tFade = 0f; float tHold = slowMoHold;
        if (fadeGroup) fadeGroup.blocksRaycasts = true;
        while ((tFade < fadeDuration) || (tHold > 0f))
        {
            if (fadeGroup && tFade < fadeDuration)
            {
                tFade += Time.unscaledDeltaTime;
                fadeGroup.alpha = Mathf.Clamp01(tFade / fadeDuration);
            }
            if (tHold > 0f) tHold -= Time.unscaledDeltaTime;
            yield return null;
        }


        if (GameTimer.Instance != null) GameTimer.Instance.Stop();
>>>>>>> d039317d713cb09fd8ef2ef750340e470f42d285
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35
        OnDied?.Invoke();
        Time.timeScale = 0f;
        if (deathMenuRoot)
            deathMenuRoot.SetActive(true);
    }

    public void TakeHeal(int amount)
    {
        if (CurrentHealth <= 0 || isDying)
            return;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        OnDamaged?.Invoke(CurrentHealth); // UI’ı günceller
<<<<<<< HEAD
=======
    }

    public void TakeHeal(int amount)
    {
        if (CurrentHealth <= 0 || isDying) return;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        OnDamaged?.Invoke(CurrentHealth); // UI’ı günceller
>>>>>>> 35feadc387ebfa3823b983aba92eb4b0fbca9f35
    }
}
