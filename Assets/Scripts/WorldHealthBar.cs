/*

// File: Assets/Scripts/UI/WorldHealthBar.cs
using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new(0f, 2.2f, 0f);

    private Camera cam;

    private void Awake()
    {
        if (playerHealth == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p)
            {
                playerHealth = p.GetComponent<PlayerHealth>();
                if (target == null) target = p.transform;
            }
        }

        // Görünürlük: başta ZORLA açık tut
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        // Image tipini garantiye al
        if (fillImage != null && fillImage.type != Image.Type.Filled)
            fillImage.type = Image.Type.Filled;

        cam = Camera.main;

        // İlk fill'i burada da yap
        ForceUpdateFill();

        // Sadece hasarda güncellemek yetmez; yine de abone olalım
        if (playerHealth != null)
            playerHealth.OnDamaged += _ => ForceUpdateFill();
    }

    private void Start()
    {
        // Script init sıralamasından bağımsız: ilk karede de güncelle
        ForceUpdateFill();
    }

    private void LateUpdate()
    {
        if (playerHealth == null) return;

        // Konum + yüzünü kameraya çevir
        if (target != null)
        {
            if (cam == null) cam = Camera.main;
            transform.position = target.position + offset;
            if (cam != null)
                transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }

    private void ForceUpdateFill()
    {
        if (fillImage == null || playerHealth == null) return;

        fillImage.fillAmount = playerHealth.Normalized;

        // Sadece ÖLÜYKEN gizle; bunun dışında açık kalsın
        bool isDead = playerHealth.CurrentHealth <= 0;
        // Objeyi kapatmak yerine istersen alpha ile gizlemek de mümkün;
        // ama basit tutuyoruz:
        gameObject.SetActive(!isDead);
    }
}

*/

using UnityEngine;
using UnityEngine.UI;


public class WorldHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2.2f, 0f);


    private Camera cam;


    private void Awake()
    {
        if (playerHealth == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p)
            {
                playerHealth = p.GetComponent<PlayerHealth>();
                if (target == null) target = p.transform;
            }
        }
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        if (fillImage != null && fillImage.type != Image.Type.Filled) fillImage.type = Image.Type.Filled;
        cam = Camera.main;
        ForceUpdateFill();
        if (playerHealth != null) playerHealth.OnDamaged += _ => ForceUpdateFill();
    }


    private void Start() { ForceUpdateFill(); }


    private void LateUpdate()
    {
        if (playerHealth == null) return;
        if (target != null)
        {
            if (cam == null) cam = Camera.main;
            transform.position = target.position + offset;
            if (cam != null) transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }

    private void ForceUpdateFill()
    {
        if (fillImage == null || playerHealth == null) return;
        fillImage.fillAmount = playerHealth.Normalized;
        bool isDead = playerHealth.CurrentHealth <= 0;
        gameObject.SetActive(!isDead);
    }
}