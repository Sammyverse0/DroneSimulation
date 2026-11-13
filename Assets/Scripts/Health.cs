using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] GameObject deathEffect;
    [SerializeField] UnityEvent onDeath;
    [SerializeField] AudioClip hitClip;

    [Header("Health Bar")]
    [SerializeField] Image healthFill;
    [SerializeField] Color fullColor = Color.green;
    [SerializeField] Color lowColor = Color.red;

    [Header("Damage Flash Settings")]
    [SerializeField] Image RedFlash;
    [SerializeField] Color flashColor = new Color(1f, 0f, 0f, 0.5f);
    [SerializeField] float fadeSpeed = 5f;

    float current;

    void Awake()
    {
        current = maxHealth;
        UpdateBar();

        if (RedFlash != null)
        {
            RedFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        }
    }

    void Update()
    {
        if (RedFlash != null && RedFlash.color.a > 0)
        {
            float newAlpha = Mathf.Lerp(RedFlash.color.a, 0f, fadeSpeed * Time.deltaTime);
            RedFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, newAlpha);
        }
    }

    public void TakeDamage(float amount)
    {
        if (current <= 0f) return;

        if (gameObject.CompareTag("Player"))
        {
            if (RedFlash != null)
            {
                RedFlash.color = flashColor;
            }
        }

        current = Mathf.Max(current - amount, 0f);
        UpdateBar();

        if (current <= 0f)
        {
            Die();
        }
        else if (hitClip != null)
        {
            AudioSource.PlayClipAtPoint(hitClip, transform.position);
        }
    }

    void UpdateBar()
    {
        if (healthFill == null) return;

        float ratio = Mathf.Clamp01(current / maxHealth);
        healthFill.fillAmount = ratio;
        healthFill.color = Color.Lerp(lowColor, fullColor, ratio);
    }

    void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        onDeath.Invoke();
        Destroy(gameObject);
    }
}